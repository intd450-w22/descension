using System;
using Actor.Interface;
using Actor.Player;
using Managers;
using UnityEngine;
using UnityEngine.Analytics;
using Util.Enums;
using Util.Helpers;

namespace Actor.AI.States
{
    public class MeleeAttackState : AIState
    {
        [Header("Settings")]
        public float knockBack;
        public float damage = 10;
        public float attackDelay = 1;
        public float attackRange = 5;
        public float attackBoxWidth = 8;
        public float postDelay = 1;
        
        [Header("Transitions")]
        public AIState onComplete;
        
        private Vector3 _direction;
        private Vector3 _target;
        private float _attackTime;
        private float _endTime;
        private bool _executed;

        private float _swingAngle;

        public override void StartState()
        {
            Speed = 0;
            Velocity = Vector3.zero;
            _target = PlayerPosition;
            _direction = (_target - Position).normalized;
            _attackTime = Time.time + attackDelay;
            _endTime = _attackTime + postDelay;
            _executed = false;
            _swingAngle = -90;
            UpdateWeaponTransform(_target, _swingAngle);
            SoundManager.Swing();
        }

        public override void EndState() {}

        public override void UpdateState()
        {
            if (!_executed && Time.time >= _attackTime)
            { 
                UpdateWeaponTransform(_target, _swingAngle += 55);
                if (_swingAngle >= 0) Execute();
            }
            else if (Time.time >= _endTime) OnComplete();
        }

        private void Execute()
        {
            _executed = true;
            
            var angle = _direction.ToDegrees();
            var box = new Vector2(1, attackBoxWidth);
            var range = attackRange - box.x / 2f;

            RaycastHit2D rayCast = Physics2D.BoxCast(Position, box, angle, _direction, range, (int) UnityLayer.Player);
            if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
            {
                rayCast.transform.GetComponent<IDamageable>().InflictDamage(damage, _direction, knockBack);
                
                Debug.Log("Attack Hit!");
                DebugHelper.DrawBoxCast2D(Position, box, angle, _direction, range, 0.5f, Color.red);
            }
            else
            {
                DebugHelper.DrawBoxCast2D(Position, box, angle, _direction, range, 0.5f, Color.yellow);
            }
        }

        private void OnComplete() => ChangeState(onComplete);
    }
}
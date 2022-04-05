using System;
using Actor.Interface;
using Actor.Player;
using UnityEngine;
using UnityEngine.Analytics;
using Util.Enums;

namespace Actor.AI.States
{
    public class MeleeAttackState : AIState
    {
        [Header("Settings")]
        public float knockBack;
        public float damage = 10;
        public float attackDelay = 1;
        public float attackRange = 5;
        public float postDelay = 1;
        
        [Header("Transitions")]
        public AIState onComplete;
        
        private Vector3 _direction;
        private float _attackTime;
        private float _endTime;
        private bool _executed;

        public override void StartState()
        {
            Speed = 0;
            Velocity = new Vector3();
            _direction = (PlayerPosition - Position).normalized;
            _attackTime = Time.time + attackDelay;
            _endTime = _attackTime + postDelay;
            _executed = false;
        }

        public override void EndState() {}

        public override void UpdateState()
        {
            if (!_executed && Time.time >= _attackTime) Execute();
            else if (Time.time >= _endTime) OnComplete();
        }

        private void Execute()
        {
            _executed = true;
            RaycastHit2D rayCast = Physics2D.BoxCast(Position, new Vector2(1, 1), 0, _direction, attackRange, (int) UnityLayer.Player);
            if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
            {
                rayCast.transform.GetComponent<IDamageable>().InflictDamage(damage, _direction, knockBack);
                
                Debug.Log("Attack Hit!");
                Debug.DrawLine(Position, rayCast.point, Color.red, 3);
            }
            else
            {
                Debug.DrawLine(Position, Position + _direction*attackRange, Color.yellow,3 );
            }
        }

        private void OnComplete() => ChangeState(onComplete);
    }
}
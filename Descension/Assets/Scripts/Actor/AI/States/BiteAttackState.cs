using System;
using Actor.Interface;
using Actor.Player;
using Managers;
using UnityEngine;
using UnityEngine.Analytics;
using Util.Enums;
using Util.Helpers;
using static Animation.AnimationListener;
using static Animation.AnimationListener.AnimationEvent;

namespace Actor.AI.States
{
    public class BiteAttackState : AIState
    {
        [Header("Settings")]
        public float knockBack;
        public float damage = 10;
        public float attackBoxWidth = 8;
        public float postDelay = 1;
        
        [Header("Transitions")]
        public AIState onComplete;
        
        // state
        private float _endTime;
        private bool _complete;
        private bool _colliderIsTrigger;
        private int _animatorIsBiting;
        private int AnimatorIsBiting => _animatorIsBiting != 0 ? _animatorIsBiting : _animatorIsBiting = Animator.StringToHash("IsBiting");
        
        new void Awake()
        {
            base.Awake();

            _colliderIsTrigger = Collider.isTrigger;
            
            AnimationListener.SetOnBroadcast(e =>
            {
                switch (e)
                {
                    case ExecuteAttack:
                    {
                        InflictDamage();
                        break;
                    }
                    case FinishAttack:
                    {
                        Animator.SetBool(AnimatorIsBiting, false);
                        _complete = true;
                        break;
                    }
                }
            });
        }

        private void InflictDamage()
        {
            var box = new Vector2(attackBoxWidth, attackBoxWidth);

            RaycastHit2D rayCast = Physics2D.BoxCast(Position, box, 0, Vector2.zero, 0, (int) traceLayers);
            if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
            {
                rayCast.transform.GetComponent<IDamageable>().InflictDamage(damage, PlayerPosition - Position, knockBack);
                
                GameDebug.Log("Attack Hit!");
                GameDebug.DrawBoxCast2D(Position, box, 0, Vector2.zero, 0, 0.5f, Color.red);
            }
            else
            {
                GameDebug.DrawBoxCast2D(Position, box, 0, Vector2.zero, 0, 0.5f, Color.yellow);
            }
        }

        public override void StartState()
        {
            _complete = false;
            _endTime = Time.time + postDelay;

            Velocity = (PlayerPosition - Position) * 3;
            Speed = Velocity.magnitude;
            Collider.isTrigger = true;
            SoundManager.MonsterBite();
            Animator.SetBool(AnimatorIsBiting, true);
            SetDestination(PlayerPosition);
        }

        public override void EndState() => Collider.isTrigger = _colliderIsTrigger;

        public override void UpdateState()
        {
            if (_complete && Time.time >= _endTime) ChangeState(onComplete);
        }
    }
}
using System;
using Actor.Interface;
using Actor.Player;
using Managers;
using UnityEngine;
using UnityEngine.Analytics;
using Util.Enums;
using Util.Helpers;
using static Actor.AI.AnimationListener;
using static Actor.AI.AnimationListener.AnimationEvent;

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
        private Vector3 _target;
        private float _endTime;
        private bool _complete;
        private int _animatorIsBiting;
        private int AnimatorIsBiting => _animatorIsBiting != 0 ? _animatorIsBiting : _animatorIsBiting = Animator.StringToHash("IsBiting");

        new void Awake()
        {
            base.Awake();
            
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
                        // Controller.gameObject.GetChildObjectWithName("Sprite").GetComponent<Rigidbody2D>().simulated = true;
                        Controller.animator.SetBool(AnimatorIsBiting, false);
                        _complete = true;
                        break;
                    }
                }
            });
        }

        private void InflictDamage()
        {
            var box = new Vector2(attackBoxWidth, attackBoxWidth);

            RaycastHit2D rayCast = Physics2D.BoxCast(Position, box, 0, Vector2.zero, 0, (int) UnityLayer.Player);
            if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
            {
                rayCast.transform.GetComponent<IDamageable>().InflictDamage(damage, PlayerPosition - Position, knockBack);
                
                Debug.Log("Attack Hit!");
                DebugHelper.DrawBoxCast2D(Position, box, 0, Vector2.zero, 0, 0.5f, Color.red);
            }
            else
            {
                DebugHelper.DrawBoxCast2D(Position, box, 0, Vector2.zero, 0, 0.5f, Color.yellow);
            }
        }

        public override void StartState()
        {
            _target = PlayerPosition;
            Velocity = (_target - Position) * 3;
            Speed = Velocity.magnitude;
            _endTime = Time.time + postDelay;
            _complete = false;
            // Controller.gameObject.GetChildObjectWithName("Sprite").GetComponent<Rigidbody2D>().simulated = false;
            Controller.animator.SetBool(AnimatorIsBiting, true);
            SetDestination(PlayerPosition);
        }

        public override void EndState() {}

        public override void UpdateState()
        {
            if (_complete && Time.time >= _endTime) ChangeState(onComplete);
        }
    }
}
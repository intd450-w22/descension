using System.Collections.Generic;
using Actor.Player;
using UnityEngine;
using UnityEngine.Diagnostics;
using Util.Enums;

namespace Actor.AI.States
{
    public class MeleeAttackState : AIState
    {
        public AIState onComplete;
        public float knockBack;
        public float damage = 10;
        public float attackDelay = 1;
        public float attackRange = 5;
        public float postDelay = 1;
        
        private bool _attackStarted;
        private Vector3 _direction;
        public new void Start()
        {
            base.Start();
        }

        public override void Initialize()
        {
            Debug.Log("Melee Attack");
            Controller.agent.speed = 0;
            _attackStarted = false;
        }

        public override void UpdateState()
        {
            if (_attackStarted) return;
            _attackStarted = true;
            
            _direction = (Controller.player.transform.position - Controller.position).normalized;
            
            Invoke(nameof(Execute), attackDelay);
        }

        public void Execute()
        {
            int mask = (int) UnityLayer.Player;
            RaycastHit2D rayCast = Physics2D.BoxCast(Controller.position, new Vector2(1, 1), 0, _direction, attackRange, mask);
            if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
            {
                Controller.player.GetComponent<PlayerController>().InflictDamage(gameObject, damage, knockBack);
                Debug.Log("Attack Hit!");
                Debug.DrawLine(Controller.position, rayCast.point, Color.red, 3);
            }
            else
            {
                Debug.DrawLine(Controller.position, Controller.position + _direction*attackRange, Color.yellow,3 );
            }

            Invoke(nameof(OnComplete), postDelay);
        }

        public void OnComplete()
        {
            Controller.SetState(onComplete);
        }
    }
}
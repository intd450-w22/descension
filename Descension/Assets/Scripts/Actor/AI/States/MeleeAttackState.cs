using Actor.Player;
using UnityEngine;
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
        

        public override void StartState()
        {
            Speed = 0;
            _direction = (PlayerPosition - Position).normalized;
            Invoke(nameof(Execute), attackDelay);
        }
        
        public override void EndState(){}

        public override void UpdateState(){}

        public void Execute()
        {
            RaycastHit2D rayCast = Physics2D.BoxCast(Position, new Vector2(1, 1), 0, _direction, attackRange, (int) UnityLayer.Player);
            if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
            {
                PlayerController.InflictDamageStatic(gameObject, damage, knockBack);
                Debug.Log("Attack Hit!");
                Debug.DrawLine(Position, rayCast.point, Color.red, 3);
            }
            else
            {
                Debug.DrawLine(Position, Position + _direction*attackRange, Color.yellow,3 );
            }

            Invoke(nameof(OnComplete), postDelay);
        }

        public void OnComplete()
        {
            ChangeState(onComplete);
        }
    }
}
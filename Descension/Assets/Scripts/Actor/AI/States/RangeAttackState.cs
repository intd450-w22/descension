using Actor.Objects;
using UnityEngine;
using Util.Enums;

namespace Actor.AI.States
{
    public class RangeAttackState : AIState
    {
        [Header("Settings")]
        public GameObject projectilePrefab;
        public float damage = 10;
        public float attackDelay = 1;
        public float postDelay = 0.5f;
        
        [Header("Transitions")]
        public AIState onComplete;
        
        private bool _attackStarted;
        
        public override void StartState()
        {
            Speed = 0;
            Invoke(nameof(Execute), attackDelay);
        }

        public override void EndState(){}

        public override void UpdateState(){}

        public void Execute()
        {
            Vector3 direction = PlayerPosition - Position; 
            Projectile.Instantiate(projectilePrefab, Position, direction, damage, Tag.Player);
            Invoke(nameof(OnComplete), postDelay);
        }

        public void OnComplete()
        {
            ChangeState(onComplete);
        }
    }
}
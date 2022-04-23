using Actor.Projectiles;
using UnityEngine;
using Util.Enums;

namespace Actor.AI.States
{
    public class RangeAttackState : AIState
    {
        [Header("Settings")]
        public GameObject projectilePrefab;
        public float damage = 10;
        public float knockBack = 50;
        public float attackDelay = 1;
        public float postDelay = 0.5f;
        
        [Header("Transitions")]
        public AIState onComplete;
        
        private float _attackTime;
        private float _endTime;
        private bool _executed;
        
        public override void StartState()
        {
            Speed = 0;
            Velocity = new Vector3();
            _attackTime = Time.time + attackDelay;
            _endTime = _attackTime + postDelay;
            _executed = false;
        }

        public override void EndState(){}

        public override void UpdateState()
        {
            if (!_executed && Time.time >= _attackTime) Execute();
            else if (Time.time >= _endTime) OnComplete();
        }

        private void Execute()
        {
            _executed = true;
            Vector3 direction = PlayerPosition - Position; 
            Projectile.Instantiate(projectilePrefab, Position, direction, damage, knockBack, Tag.Player);
        }

        private void OnComplete() => ChangeState(onComplete);
    }
}
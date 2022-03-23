using Actor.Objects;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;

namespace Actor.AI.States
{
    public class RangeAttackState : AIState
    {
        public AIState onComplete;
        public GameObject projectilePrefab;
        public float damage = 10;
        public float attackDelay = 1;
        public float postDelay = 0.5f;
        
        private bool _attackStarted;

        public new void Start()
        {
            base.Start();
        }

        public override void Initialize()
        {
            Controller.agent.speed = 0;
            _attackStarted = false;
        }

        public override void UpdateState()
        {
            if (_attackStarted) return;
            _attackStarted = true;
            
            Invoke(nameof(Execute), attackDelay);
        }

        public void Execute()
        {
            Vector3 direction = GameManager.PlayerController.transform.position - Controller.position; 
            Projectile.Instantiate(projectilePrefab, Controller.position, direction, damage, Tag.Player);
            Invoke(nameof(OnComplete), postDelay);
        }

        public void OnComplete()
        {
            Controller.SetState(onComplete);
        }
    }
}
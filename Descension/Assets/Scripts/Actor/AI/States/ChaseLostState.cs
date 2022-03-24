using UnityEngine;
using Util.Enums;

namespace Actor.AI.States
{
    public class ChaseLostState : AIState
    {
        [Header("Settings")]
        public float reachThreshold = 5;
        public float sightDistance = 15;
        public float speed = 10;
        
        [Header("Transitions")]
        public AIState onPlayerSpotted;
        public AIState onPlayerLost;
        
        private Vector3 _target;

        
        public override void StartState()
        {
            Speed = speed;
            _target = PlayerPosition;
            SetDestination(_target);
        }

        public override void EndState() {}

        public override void UpdateState()
        {
            Vector3 toTarget = _target - Position;
            
            RaycastHit2D rayCast = Physics2D.Raycast(Position, toTarget.normalized, sightDistance, (int)~UnityLayer.Enemy);
            if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
            {
                ChangeState(onPlayerSpotted);
                return;
            }
            
            Debug.DrawLine(Position, _target, Color.yellow);

            if (toTarget.magnitude < reachThreshold) Look();
        }
        
        // Look in all directions to see if Player is still visible
        private void Look()
        {
            RaycastHit2D rayCast = Physics2D.Raycast(Position, (PlayerPosition - Position).normalized, sightDistance, (int)~UnityLayer.Enemy);
            if (rayCast && rayCast.collider.gameObject.CompareTag("Player"))
            {
                ChangeState(onPlayerSpotted);
            }
            else
            {
                ChangeState(onPlayerLost);
            }
        }
    }
}
using UnityEngine;
using Util.Enums;
using Util.Helpers;

namespace Actor.AI.States
{
    public class ChaseState : AIState
    {
        [Header("Settings")]
        public float reachThreshold = 5;
        public float sightDistance = 15;
        public float speed = 10;
        
        [Header("Transitions")]
        public AIState onPlayerReached;
        public AIState onPlayerLost;
        
        private Vector3 _target;
        private Vector3 _direction;
  
        public override void StartState()
        {
            Speed = speed;
            _target = PlayerPosition;
            _direction = (PlayerPosition - Position).normalized;
        }

        public override void EndState() {}

        public override void UpdateState()
        {
            Vector3 position = Position;
            Vector3 toTarget = _target - position;
            
            UpdateWeaponTransform(_target);

            RaycastHit2D rayCast = Physics2D.Raycast(Position, toTarget.normalized, sightDistance, (int)~UnityLayer.Enemy);
            if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
            {
                if (toTarget.magnitude < reachThreshold)
                {
                    ChangeState(onPlayerReached);
                }
                else
                {
                    _target = rayCast.transform.position;
                    SetDestination(_target);
                    GameDebug.DrawLine(Position, rayCast.point, Color.red);
                }
            }
            else
            {
                ChangeState(onPlayerLost);
            }

            
        }
    }
}

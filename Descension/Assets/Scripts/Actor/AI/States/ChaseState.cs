using UnityEngine;
using Util.Enums;

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

        
        public override void StartState()
        {
            Speed = speed;
            _target = PlayerPosition;
        }

        public override void EndState() {}

        public override void UpdateState()
        {
            Vector3 toTarget = _target - Position;
            
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
                    Debug.DrawLine(Position, rayCast.point, Color.red);
                }
            }
            else
            {
                ChangeState(onPlayerLost);
            }

            SetDestination(_target);
        }
    }
}

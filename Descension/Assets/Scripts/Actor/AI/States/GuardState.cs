using System.Collections.Generic;
using UnityEngine;
using Util.Enums;

namespace Actor.AI.States
{
    public class GuardState : AIState
    {
        [Header("Settings")]
        public float sightDistance = 15;
        public float speed = 30;

        [Header("Transitions")]
        public AIState onPlayerSpotted;
        
        public override void StartState()
        {
            Speed = speed;
            SetDestination(PatrolTargets[0].position);
        }

        public override void EndState(){}

        public override void UpdateState()
        {
            Vector3 toPlayer = PlayerPosition - Position;
            if (toPlayer.magnitude < sightDistance)
            {
                RaycastHit2D rayCast = Physics2D.Raycast(Position, toPlayer.normalized, sightDistance, (int)~UnityLayer.Enemy);
                if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
                {
                    ChangeState(onPlayerSpotted);
                }
            }
        }
    }
}
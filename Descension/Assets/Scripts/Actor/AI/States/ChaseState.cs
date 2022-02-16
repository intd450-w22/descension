using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using Util.Enums;

namespace Actor.AI.States
{
    public class ChaseState : AIState
    {
        public AIState onPlayerReached;
        public AIState onPlayerLost;
        public float reachThreshold = 5;
        public float sightDistance = 15;
        public float speed = 10;
        
        private Vector3 _target;

        public new void Start()
        {
            base.Start();
        }

        public override void Initialize()
        {
            Debug.Log("Chase");
            Controller.agent.speed = speed;
            _target = Controller.player.transform.position;
        }

        public override void UpdateState()
        {
            Vector3 v = _target - Controller.position;
            Vector3 direction = v.normalized;
            float distance = v.magnitude;
            
            int mask = (int)~UnityLayer.Enemy;
            RaycastHit2D rayCast = Physics2D.BoxCast(Controller.position, new Vector2(1, 1), 0, direction, sightDistance, mask);
            if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
            {
                if (distance < reachThreshold)
                {
                    Controller.SetState(onPlayerReached);
                }
                else
                {
                    _target = rayCast.transform.position;
                    Debug.DrawLine(Controller.position, rayCast.point, Color.red);
                }
            }
            else
            {
                Controller.SetState(onPlayerLost);
            }

            Controller.agent.SetDestination(_target);
        }
    }
}

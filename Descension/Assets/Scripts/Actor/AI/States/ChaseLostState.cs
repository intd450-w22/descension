using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using Util.Enums;

namespace Actor.AI.States
{
    public class ChaseLostState : AIState
    {
        public AIState onPlayerSpotted;
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
            Debug.Log("Chase Lost");
            Controller.agent.speed = speed;
            _target = Controller.player.transform.position;
        }

        public override void UpdateState()
        {
            Vector3 v = _target - Controller.position;
            Vector3 direction = v.normalized;
            
            int mask = (int)~UnityLayer.Enemy;
            RaycastHit2D rayCast = Physics2D.BoxCast(Controller.position, new Vector2(1, 1), 0, direction, sightDistance, mask);
            if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
            {
                Controller.SetState(onPlayerSpotted);
                return;
            }
            
            Debug.DrawLine(Controller.position, _target, Color.yellow);

            float distance = v.magnitude;
            if (distance < reachThreshold)
            {
                Look();
            }
            else
            {
                // continue moving towards last position player was seen
                Controller.agent.SetDestination(_target);
            }
        }
        
        // Look in all directions to see if PlayerController is still visible
        private void Look()
        {
            Vector2 direction = (Controller.player.transform.position - Controller.position).normalized;
            int mask = (int)~UnityLayer.Enemy;
            RaycastHit2D rayCast = Physics2D.Raycast(Controller.position, direction, sightDistance, mask);
            if (rayCast && rayCast.collider.gameObject.CompareTag("Player"))
            {
                Controller.SetState(onPlayerSpotted);
            }
            else
            {
                Controller.SetState(onPlayerLost);
            }
        }
    }
}
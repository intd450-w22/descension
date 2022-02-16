using System.Collections.Generic;
using Actor.Player;
using UnityEngine;
using UnityEngine.Diagnostics;
using Util.Enums;

namespace Actor.AI.States
{
    public class GuardState : AIState
    {
        public AIState onPlayerSpotted;
        public float sightDistance = 15;
        
        public new void Start()
        {
            base.Start();
        }

        public override void Initialize()
        {
            Debug.Log("Guard");
        }

        public override void UpdateState()
        {
            Vector3 v = Controller.player.transform.position - Controller.position;
            Vector2 direction = v.normalized;
            if (v.magnitude < sightDistance)
            {
                int mask = (int)~UnityLayer.Enemy;
                RaycastHit2D rayCast = Physics2D.BoxCast(Controller.position, new Vector2(1, 1), 0, direction, sightDistance, mask);
                if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
                {
                    Controller.SetState(onPlayerSpotted);
                    return;
                }
            }
        }
    }
}
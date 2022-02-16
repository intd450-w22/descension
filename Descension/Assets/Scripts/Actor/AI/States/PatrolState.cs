using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using Util.Enums;
using static Util.Helpers.CalculationHelper;
namespace Actor.AI.States
{
    public class PatrolState : AIState
    {
        public AIState onPlayerSpotted;
        public float sightDistance = 10;
        public float speed = 10;
        public bool loopTargets;                // go to first target at end of list? or turn around if false

        private Transform _currentTarget;
        private int _patrolIndex = 0;           // index of current patrol target
        private int _patrolDirection = 1;       // tracks forward/backward for patrolling
        private List<Transform> _patrolTargets; // list of patrol targets, generated from children of PatrolTargets subobject

        public new void Start()
        {
            base.Start();
            _patrolTargets = new List<Transform>(transform.Find("PatrolTargets").GetComponentsInChildren<Transform>());
            _patrolIndex = FindClosest(Controller.position, ref _patrolTargets);
            _currentTarget = _patrolTargets[_patrolIndex];
        }

        public override void Initialize()
        {
            Debug.Log("Patrol");
            Controller.agent.speed = speed;
        }

        public override void UpdateState()
        {
            int mask = (int)~UnityLayer.Enemy;
            var rayCast = Physics2D.BoxCast(Controller.position, new Vector2(1, 1), 0, Controller.agent.velocity.normalized, sightDistance, mask);
            if (rayCast)
            {
                if (rayCast.transform.gameObject.CompareTag("Player"))
                {
                    Controller.SetState(onPlayerSpotted);
                    Debug.DrawLine(Controller.position, rayCast.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(Controller.position, rayCast.point, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(Controller.position, Controller.agent.velocity.normalized * sightDistance, Color.green);
            }
        
            float distance = (_currentTarget.position - Controller.position).magnitude;
            if (distance > 2)
            {
                Controller.agent.SetDestination(_currentTarget.position);
            }
            else
            {
                GetNextTarget();
            }
        }
    
        // sets target to next in patrolTargets list
        private void GetNextTarget()
        {
            _patrolIndex += _patrolDirection;
            if (loopTargets)
            {
                if (_patrolIndex == _patrolTargets.Count) _patrolIndex = 0;
                else if (_patrolIndex == -1) _patrolIndex = _patrolTargets.Count - 1;
            }
            else
            {
                if (_patrolIndex == _patrolTargets.Count || _patrolIndex == -1)
                {
                    _patrolDirection = -_patrolDirection;
                    _patrolIndex += 2 * _patrolDirection;
                }
            }
            
            _currentTarget = _patrolTargets[_patrolIndex];
        }
    }
}

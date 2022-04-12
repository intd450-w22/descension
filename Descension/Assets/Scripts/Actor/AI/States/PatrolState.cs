using System;
using UnityEngine;
using Util.EditorHelpers;
using Util.Enums;
using Util.Helpers;
using static Util.Helpers.CalculationHelper;
namespace Actor.AI.States
{
    public class PatrolState : AIState
    {
        [Header("Settings")] 
        public float sightSwayDegrees = 30;
        public float sightSwaySpeed = 1;
        public float sightDistance = 10;
        public float movementSpeed = 10;
        public float reachThreshold = 3;
        public bool loopTargets;                // go to first target at end of list? or turn around if false
        
        [Header("Transitions")]
        public AIState onPlayerSpotted;
        
        [Header("This state patrol between child objects of PatrolTargets")]
        [SerializeField, ReadOnly] private Transform currentTarget;
        
        private int _patrolIndex;               // index of current patrol target
        private int _patrolDirection = 1;       // tracks forward/backward for patrolling
        private Vector3 _lookDirection;
        
        public override void StartState()
        {
            Speed = movementSpeed;
            _patrolIndex = FindClosest(Position, PatrolTargets);
            currentTarget = PatrolTargets[_patrolIndex];
            
            var position = currentTarget.position;
            UpdateWeaponTransform(position);
            SetDestination(position);
        }

        public override void EndState(){}

        public override void UpdateState()
        {
            // add side to side sway to sight
            _lookDirection = Velocity.normalized.GetRotated((float) Math.Cos(Time.time * sightSwaySpeed) * sightSwayDegrees);
            
            var rayCast = Physics2D.Raycast(Position, _lookDirection, sightDistance, (int)~UnityLayer.Enemy);
            if (rayCast)
            {
                if (rayCast.transform.gameObject.CompareTag("Player"))
                {
                    ChangeState(onPlayerSpotted);
                    Debug.DrawLine(Position, rayCast.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(Position, rayCast.point, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(Position, _lookDirection * sightDistance, Color.green);
            }
        
            if ((currentTarget.position - Position).magnitude < reachThreshold) GetNextTarget();
        }
    
        // sets target to next in patrolTargets list
        private void GetNextTarget()
        {
            _patrolIndex += _patrolDirection;
            if (loopTargets)
            {
                if (_patrolIndex == PatrolTargets.Count) _patrolIndex = 0;
                else if (_patrolIndex == -1) _patrolIndex = PatrolTargets.Count - 1;
            }
            else
            {
                if (_patrolIndex == PatrolTargets.Count || _patrolIndex == -1)
                {
                    _patrolDirection = -_patrolDirection;
                    _patrolIndex += 2 * _patrolDirection;
                }
            }

            _patrolIndex = SafeIndex(_patrolIndex, PatrolTargets.Count);
            currentTarget = PatrolTargets[_patrolIndex];

            var position = currentTarget.position;
            UpdateWeaponTransform(position);
            SetDestination(position);
        }
    }
}

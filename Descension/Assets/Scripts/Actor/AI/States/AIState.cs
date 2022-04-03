using System.Collections.Generic;
using Actor.Player;
using Managers;
using UnityEngine;

namespace Actor.AI.States
{
    public abstract class AIState : MonoBehaviour
    {
        private AIController _controller;
        private AIController Controller
        {
            get
            {
                if (!_controller) _controller = GetComponent<AIController>();
                return _controller;
            }
        }
        
        // protected PlayerController Player => PlayerController.Instance;
        protected Vector3 PlayerPosition => PlayerController.Position;
        protected Vector3 Position => Controller.agent.transform.position;
        protected Vector3 Velocity => Controller.agent.velocity;
        protected float Speed { set => Controller.agent.speed = value; }

        private List<Transform> _patrolTargets;
        protected List<Transform> PatrolTargets
        {
            get
            {
                if (_patrolTargets == null)
                {
                    _patrolTargets = new List<Transform>(transform.Find("PatrolTargets").GetComponentsInChildren<Transform>());
                    _patrolTargets.Remove(transform.Find("PatrolTargets"));
                }
                return _patrolTargets;
            }
        }

        public void Awake()
        {
            enabled = false;
        }

        protected void SetDestination(Vector3 destination)
        {
            Controller.agent.SetDestination(destination);
        }

        protected void ChangeState(AIState newState)
        {
            _controller.SetState(newState);
        }

        // called every frame 
        public abstract void UpdateState();
        
        // called when changing to this state
        public abstract void StartState();
        
        // called when changing to a different state
        public abstract void EndState();
    }
}

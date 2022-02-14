using System;
using System.Collections.Generic;
using Actor.Player;
using UnityEngine;
using UnityEngine.AI;
using Util.Enums;
using static Util.Helpers.CalculationHelper;
using static Util.Helpers.Extensions;

namespace Actor.AI
{
    // General controller class for enemy AI. Add a script derived from AttackBase to the enemy object to implement unique attack logic.
    public class AIController : MonoBehaviour
    {
        public float hitPoints = 100;
        public float damage = 10;
        public GameObject floatingTextDialogue;
        public GameObject floatingDamageDialogue;
        
        public StateAttributes patrollingAttributes; // attributes when in patrolling state
        public StateAttributes chasingAttributes;    // attributes when in chasing state

        public bool chasePlayer;    // chase PlayerController on sight?
        public bool loopTargets;    // go to first target at end of list? or turn around if false
        public State currentState;

        private Transform _currentTarget;       // current target reticle
        private List<Transform> _patrolTargets; // list of patrol targets, generated from children of PatrolTargets subobject
        private AttackBase _attack;             // attack script
        private NavMeshAgent _agent;            // agent script
        private Transform _player;              // PlayerController position
        private Vector3 _position;              // cached current position of agent
        private bool _alive;
        private int _patrolIndex = 0;           // index of current patrol target
        private int _patrolDirection = 1;       // tracks forward/backward for patrolling
        private StateAttributes _attributes;
        
        
        void Start()
        {
            if (!FindObjectOfType<NavMeshSurface2d>())
            {
                Debug.LogWarning("Need to add NavMeshPrefab to map and bake to use enemy. Also add NavMeshModifier to Ground and Walls of the Grid.");
                _alive = false;
                Destroy(this);
                return;
            }
            
            _agent = GetComponentInChildren<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            
            _patrolTargets = new List<Transform>(transform.Find("PatrolTargets").GetComponentsInChildren<Transform>());
            _currentTarget = gameObject.GetChildTransformWithName("CurrentTarget");
            _player = FindObjectOfType<PlayerController>().transform;
            
            _alive = true;
            
            StartPatrol();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_alive) return;
            
            if (hitPoints <= 0)
            {
                OnKilled();
                return;
            }

            _position = _agent.transform.position;  // cache since it is used in multiple methods

            See();
            // Hear();
            MoveToTarget();
        }

        public void SetAttackBase(AttackBase script)
        {
            _attack = script; 
        }
        
        public virtual void InflictDamage(float dmg)
        {
            Debug.Log($"Enemy hit for {dmg} damage");
            hitPoints -= dmg;
            ShowFloatingDamageDialogue("Hp-" + dmg.ToString());
        }
        
        protected virtual void OnKilled()
        {
            _alive = false;
            Destroy(gameObject); // for now 
            // TODO change to dead sprite / make body searchable? 
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            GameObject obj = other.gameObject;
            if (obj.CompareTag("Player"))
            {
                obj.GetComponent<PlayerController>().InflictDamage(damage);
            }
        }

        private void SetAttributes(StateAttributes attributes)
        {
            _attributes = attributes;
            _agent.speed = attributes.speed;
        }

        // Starts chase state
        private void StartChase(GameObject obj)
        {
            currentState = State.Chasing;
            SetAttributes(chasingAttributes);
            SetTarget(obj.transform);
        }

        // Starts patrolling state. If no parameter input, goes to closest patrol target.
        private void StartPatrol(int index = -1)
        {
            currentState = State.Patrolling;
            
            if (index < 0) index = FindClosest(_position, ref _patrolTargets);
            
            SetAttributes(patrollingAttributes);
            SetTarget(_patrolTargets[index]);
        }

        
        private void SetTarget(Transform t)
        {
            _currentTarget.position = t.position;
        }
            
    
        // Raycast forward to look for PlayerController, sets PlayerController as target if detected. Could modify mask to detect PlayerController lighting
        private void See()
        {
            RaycastHit2D rayCast;
            int mask = (int)~UnityLayer.Enemy;
            
            switch (currentState)
            {
                case State.Patrolling:
                {
                    rayCast = Physics2D.BoxCast(_position, new Vector2(1, 1), 0, _agent.velocity.normalized, _attributes.sightDistance, mask);
                    if (rayCast)
                    {
                        if (rayCast.transform.gameObject.CompareTag("Player"))
                        {
                            if (chasePlayer)
                            {
                                StartChase(rayCast.transform.gameObject);
                            }
                            Debug.DrawLine(_position, rayCast.point, Color.red);
                        }
                        else
                        {
                            Debug.DrawLine(_position, rayCast.point, Color.yellow);
                        }
                    }
                    else
                    {
                        Debug.DrawRay(_position, _agent.velocity.normalized * _attributes.sightDistance, Color.green);
                    }
                    
                    break;
                }
                    
                
                case State.Chasing:
                {
                    Vector3 direction = (_currentTarget.position - _position).normalized;

                    rayCast = Physics2D.BoxCast(_position, new Vector2(1, 1), 0, direction, _attributes.sightDistance, mask);
                    if (rayCast && rayCast.transform.gameObject.CompareTag("Player"))
                    {
                        _currentTarget.position = rayCast.transform.position;
                        Debug.DrawLine(_position, rayCast.point, Color.red);
                    }
                    else
                    {
                        currentState = State.ChasingLost;
                        Debug.DrawLine(_position, rayCast.point, Color.yellow);
                    }

                    break;
                }

                
                case State.ChasingLost:
                {
                    Debug.DrawLine(_position, _currentTarget.position, Color.yellow);
                    break;
                }
                
                case State.Attacking:
                    break;
            }
        }
    
        private void Hear()
        {
            // TODO implement hearing
        }

        // Look in all directions to see if PlayerController is still visible
        private void Look()
        {
            Vector3 direction = (_player.position - _position).normalized;
            
            int mask = (int)~UnityLayer.Enemy;
            RaycastHit2D rayCast = Physics2D.Raycast(_position, direction, _attributes.sightDistance, mask);
            if (rayCast && rayCast.collider.gameObject.CompareTag("Player"))
            {
                // PlayerController spotted, lock on again
                Debug.DrawLine(_position, rayCast.point, Color.red, 2);
                Debug.Log("Target found!");

                if (currentState != State.Chasing)
                {
                    StartChase(rayCast.rigidbody.gameObject);
                }
            }
            else
            {
                Debug.DrawRay(_position, direction * _attributes.sightDistance, Color.yellow, 1);

                // patrol if PlayerController not found
                Debug.Log("Target lost");
                StartPatrol();
            }
        }
        
        // process movement towards current target location
        private void MoveToTarget()
        {
            float distance = (_currentTarget.position - _position).magnitude;

            if (_attack != null && currentState == State.Chasing && distance < _attack.range)
            {
                if (_attack != null) _attack.Execute();
            }
            
            if (distance > 1)
            {
                _agent.SetDestination(_currentTarget.position);
            }
            else
            {
                OnTargetReached();
            }
        }

        // executed when the target is reached
        private void OnTargetReached()
        {
            switch (currentState)
            {
                case State.Patrolling:
                {
                    GetNextTarget();
                    break;
                }

                case State.Chasing:
                {
                    if (_attack != null) _attack.Execute();
                    break;
                }

                case State.ChasingLost:
                {
                    Look();
                    break;
                }

                case State.Attacking:
                {
                
                    break;
                }
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
            
            SetTarget(_patrolTargets[_patrolIndex]);
        }
    
        private void TurnAround()
        {
            _patrolDirection = -_patrolDirection;
            GetNextTarget();
        }

        private void ShowFloatingTextDialogue(string text)
        {
            var t = Instantiate(floatingDamageDialogue, _position, Quaternion.identity);
            t.GetComponent<TextMesh>().text = text;
        }
        
        private void ShowFloatingDamageDialogue(string text)
        {
            var t = Instantiate(floatingTextDialogue, _position, Quaternion.identity);
            t.GetComponent<TextMesh>().text = text;
        }
    }

    public enum State
    {
        Patrolling,
        Chasing,
        ChasingLost,
        Attacking
    }

    [Serializable]
    public struct StateAttributes
    {
        public float speed;         // how fast to move in this state
        public float sightDistance; // how far is vision in this state
        public float pauseTime;     // how long to pause when current target is reached
    }
}
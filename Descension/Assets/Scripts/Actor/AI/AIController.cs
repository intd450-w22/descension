using System;
using System.Collections;
using System.Collections.Generic;
using Actor.Player;
using UnityEngine;
using static Util;
using UnityEngine.AI;

namespace Actor.AI
{
    public class AIController : MonoBehaviour
    {
        public NavMeshAgent agent;
        public float hitPoints = 100;
        public float damage = 10;
        public GameObject floatingTextDialogue;
        public GameObject floatingDamageDialogue;

        public AttackBase attack;
        // patrolling state
        public Transform currentTarget;    // must set to object to be used as target
        public StateAttributes patrollingAttributes; // attributes when in patrolling state
        public StateAttributes chasingAttributes;   // attributes when in chasing state

        public bool chasePlayer;    // chase player on sight?
        public bool loopTargets;    // go to first target at end of list? or turn around if false
        public State currentState;
        public List<Transform> patrolTargets;

        // internal state
        private float _attackRange;
        private bool _alive = true;
        private int _patrolIndex = 0;           // index of current patrol target
        private int _patrolDirection = 1;       // tracks forward/backward for patrolling
        private StateAttributes _attributes;
    
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            
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
            
            See();
            MoveToTarget();
        }
        
        public void InflictDamage(float dmg)
        {
            hitPoints -= dmg;
            ShowFloatingDamageDialogue("Hp-" + dmg.ToString());
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            GameObject obj = other.gameObject;
            if (obj.CompareTag("Player"))
            {
                obj.GetComponent<player>().inflictDamage(damage);
            }
        }

        private void OnKilled()
        {
            _alive = false;
            // TODO change to dead sprite / make body searchable? 
        }

        private void SetAttributes(StateAttributes attributes)
        {
            _attributes = attributes;
            agent.speed = attributes.speed;
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
            if (index < 0)
            {
                index = FindClosest(transform.position, ref patrolTargets);
            }
            
            SetAttributes(patrollingAttributes);
            SetTarget(patrolTargets[index]);
        }

        
        private void SetTarget(Transform t)
        {
            currentTarget.position = t.position;
        }
            
    
        // Raycast forward to look for player, sets player as target if detected
        private void See()
        {
            RaycastHit2D rayCast;
            
            switch (currentState)
            {
                case State.Patrolling:
                {
                    rayCast = Physics2D.BoxCast(transform.position, new Vector2(2, 2), 0, agent.velocity.normalized, _attributes.sightDistance, (int) ~Layer.Enemy);
                    if (rayCast)
                    {
                        if (rayCast.transform.gameObject.CompareTag("Player"))
                        {
                            if (chasePlayer)
                            {
                                StartChase(rayCast.transform.gameObject);
                            }
                    
                            Debug.DrawRay(transform.position, agent.velocity.normalized * _attributes.sightDistance, Color.red);
                        }
                        else
                        {
                            Debug.DrawRay(transform.position, agent.velocity.normalized * _attributes.sightDistance, Color.yellow);
                        }
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, agent.velocity.normalized * _attributes.sightDistance, Color.green);
                    }
                    
                    break;
                }
                    
                
                case State.Chasing:
                {
                    Vector3 position = transform.position;
                    Vector2 direction = (currentTarget.position - position).normalized;
                    rayCast = Physics2D.BoxCast(position, new Vector2(2, 2), 0, direction, _attributes.sightDistance, (int) ~Layer.Enemy);
                    if (rayCast.transform.gameObject.CompareTag("Player"))
                    {
                        currentTarget.position = rayCast.transform.position;
                        Debug.DrawRay(transform.position, agent.velocity.normalized * _attributes.sightDistance, Color.red);
                    }
                    else
                    {
                        currentState = State.ChasingLost;
                        Debug.DrawRay(transform.position, agent.velocity.normalized * _attributes.sightDistance, Color.yellow);
                    }
                    
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

        // Look in all directions to see if player is still visible
        private void Look()
        {
            int step = 10;
            int i = -step;
            while ((i += step) < 360)
            {
                Vector2 d = Quaternion.Euler(0,0, i) * new Vector3(1, 0, 0);
                
                RaycastHit2D rayCast = Physics2D.BoxCast(transform.position, new Vector2(2, 2), 0, d, _attributes.sightDistance, (int) ~Layer.Enemy);
                if (rayCast)
                {
                    if (rayCast.collider.gameObject.CompareTag("Player"))
                    {
                        Debug.DrawRay(transform.position, d * _attributes.sightDistance, Color.red, 1);
                        Debug.Log("Target found!");

                        if (currentState != State.Chasing)
                        {
                            StartChase(rayCast.rigidbody.gameObject);
                        }

                        return;
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, d * _attributes.sightDistance, Color.yellow, 1);
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, d * _attributes.sightDistance, Color.green, 1);
                }
            }
            
            // patrol if player not found
            Debug.Log("Target lost");
            StartPatrol();
        }
        
        // process movement towards current target location
        private void MoveToTarget()
        {
            float distance = (currentTarget.position - transform.position).magnitude;

            if (currentState == State.Chasing && distance < _attackRange)
            {
                if (attack != null) attack.Execute();
            }
            
            if (distance > 1)
            {
                agent.SetDestination(currentTarget.position);
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
                    if (attack != null) attack.Execute();
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
                if (_patrolIndex == patrolTargets.Count) _patrolIndex = 0;
                else if (_patrolIndex == -1) _patrolIndex = patrolTargets.Count - 1;
            }
            else
            {
                if (_patrolIndex == patrolTargets.Count || _patrolIndex == -1)
                {
                    _patrolDirection = -_patrolDirection;
                    _patrolIndex += 2 * _patrolDirection;
                }
            }
            
            SetTarget(patrolTargets[_patrolIndex]);
        }
    
        private void TurnAround()
        {
            _patrolDirection = -_patrolDirection;
            GetNextTarget();
        }

        private void ShowFloatingTextDialogue(string text)
        {
            var t = Instantiate(floatingDamageDialogue, transform.position, Quaternion.identity);
            t.GetComponent<TextMesh>().text = text;
        }
        
        private void ShowFloatingDamageDialogue(string text)
        {
            var t = Instantiate(floatingTextDialogue, transform.position, Quaternion.identity);
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
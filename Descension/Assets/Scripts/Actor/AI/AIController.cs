using System;
using System.Collections;
using System.Collections.Generic;
using Actor.Player;
using UnityEngine;
using Util;

namespace Actor.AI
{
    public class AIController : MonoBehaviour
    {
        public float hitPoints = 100;
        public float damage = 10;
        public GameObject floatingTextDialogue;
        public GameObject floatingDamageDialogue;
        
        // patrolling state
        public GameObject currentTarget;    // must set to object to be used as target
        public StateAttributes patrollingAttributes; // attributes when in patrolling state
        public StateAttributes chasingAttributes;   // attributes when in chasing state
    
        public bool loopTargets;    // go to first target at end of list? or turn around if false
        public State currentState;
        public List<GameObject> patrolTargets;
    
        // internal state
        private bool _alive = true;
        private bool _paused;
        private float _pausedTime = 0;
        private Vector2 _forward;               // forward facing direction calculated from current movement direction
        private int _patrolIndex = 0;           // index of current patrol target
        private int _patrolDirection = 1;       // tracks forward/backward for patrolling
        private StateAttributes _attributes;
    
        // Start is called before the first frame update
        void Start()
        {
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
            // line trace forward to look for player
            See();
            HandlePause();
            if (!_paused) MoveToTarget();
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
            else
            {
                // Debug.Log("Collision!!");
                TurnAround();
                _paused = true;
            }
        }

        private void OnKilled()
        {
            _alive = false;
            // TODO change to dead sprite / make body searchable? 
        }



        private void HandlePause()
        {
            if (_paused && _pausedTime < _attributes.pauseTime)
            {
                _pausedTime += Time.deltaTime;
            }
            else
            {
                _pausedTime = 0;
                _paused = false;
            }
        }

        // Starts chase state
        private void StartChase(GameObject obj)
        {
            currentState = State.Chasing;
            _attributes = chasingAttributes;
            SetTarget(obj);
        }

        // Starts patrolling state. If no parameter input, goes to closest patrol target.
        private void StartPatrol(int index = -1)
        {
            currentState = State.Patrolling;
            _attributes = patrollingAttributes;
            if (index != -1)
            {
                _patrolIndex = index;
            }
            else
            {
                _patrolIndex = Util.Util.FindClosest(transform.position, ref patrolTargets);
            }
            Debug.Log(_patrolIndex);
            SetTarget(patrolTargets[_patrolIndex]);
        }
    
        // Raycast forward to look for player, sets player as target if detected
        private void See()
        {
            // ignore enemy layer 
            RaycastHit2D rayCast = Physics2D.BoxCast(transform.position, new Vector2(2, 2), 0, _forward, _attributes.sightDistance, (int) ~Layer.Enemy);
            if (rayCast)
            {
                if (rayCast.collider.gameObject.CompareTag("Player"))
                {
                    StartChase(rayCast.collider.gameObject);
                    Debug.DrawRay(transform.position, _forward * _attributes.sightDistance, Color.red);
                }
                else
                {
                    Debug.DrawRay(transform.position, _forward * _attributes.sightDistance, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, _forward * _attributes.sightDistance, Color.green);
            }
        }
    
        private void Hear()
        {
            // TODO implement hearing
        }

        // Look in all directions to see if player is still visible
        private void Look()
        {
            foreach (var d in Util.Util.Directions)
            {
                RaycastHit2D rayCast = Physics2D.BoxCast(transform.position, new Vector2(2, 2), 0, d, _attributes.sightDistance, (int) ~Layer.Enemy);
                if (rayCast)
                {
                    if (rayCast.collider.gameObject.CompareTag("Player"))
                    {
                        Debug.DrawRay(transform.position, d * _attributes.sightDistance, Color.red, 1);
                        Debug.Log("Target found!");

                        StartChase(rayCast.rigidbody.gameObject);
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
    
    
        // sets target to position of obj and sets facing vector
        private void SetTarget(GameObject obj)
        {
            currentTarget.transform.SetPositionAndRotation(obj.transform.position, obj.transform.rotation);
            _forward = (currentTarget.transform.position - transform.position).normalized;
            // Debug.Log(_forward);
        }
    
        // process movement towards current target location
        private void MoveToTarget()
        {
        
            Vector3 difference = currentTarget.transform.position - transform.position;
            if (difference.magnitude > 0.1)
            {
                transform.Translate(difference.normalized * _attributes.speed * Time.deltaTime);
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
                    _paused = true;
                    GetNextTarget();
                    break;
                }

                case State.Chasing:
                {
                    Look();
                    break;
                }

                case State.Attacking:
                {
                
                    break;
                }

                default:
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
            Debug.Log(_patrolIndex);
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
using System;
using System.Collections.Generic;
using Actor.AI;
using Actor.AI.States;
using Actor.Player;
using UI.Controllers;
using Util.Enums;
using UnityEngine;
using UnityEngine.AI;
using static Util.Helpers.CalculationHelper;
using static Util.Helpers.Extensions;
using Managers;
using Unity.Collections;

namespace Actor.AI
{
    // General controller class for enemy AI. Add a script derived from AttackBase to the enemy object to implement unique attack logic.
    public class AIController : MonoBehaviour
    {
        public float hitPoints = 100;
        public float touchDamage = 10;
        
        [ReadOnly] public AIState state;                     // current state in the state machine, read only
        [HideInInspector] public Vector2 forward;            // cached current forward direction
        [HideInInspector] public Vector3 position;           // cached current position
        [HideInInspector] public NavMeshAgent agent;         // agent script
        [HideInInspector] public GameObject player;          // read only reference to player
        
        private Transform _player;                          // player transform
        private bool _alive;                                // is the player alive

        private HUDController _hudController;
        
        void Start()
        {
            if (!FindObjectOfType<NavMeshSurface2d>())
            {
                Debug.LogWarning("Need to add NavMeshPrefab to map and bake to use enemy. Also add NavMeshModifier to Ground and Walls of the Grid.");
                _alive = false;
                Destroy(this);
                return;
            }
            
            player = GameObject.FindGameObjectWithTag("Player");

            if (!state)
            {
                Debug.LogWarning("Must set variable \"state\" to an attached AIState script.");
                state = GetComponent<AIState>();
            }
            
            agent = GetComponentInChildren<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            
            _player = FindObjectOfType<PlayerController>().transform;
            _hudController = UIManager.Instance.GetHudController();
            
            _alive = true;
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

            Cache();

            if (state) state.UpdateState();
        }
        
        public void SetState(AIState state)
        {
            this.state = state;
            this.state.Initialize();
        }
        
        public virtual void InflictDamage(float dmg)
        {
            Debug.Log($"Enemy hit for {dmg} damage");
            hitPoints -= dmg;
            _hudController.ShowFloatingText(position, "Hp-" + dmg, Color.red);
        }
        
        protected virtual void OnKilled()
        {
            _alive = false;
            Destroy(gameObject); // for now 
            // TODO change to dead sprite / make body searchable? 
        }
        
        private void Cache()
        {
            forward = (_player.position - position).normalized;
            position = agent.transform.position;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            GameObject obj = other.gameObject;
            if (obj.CompareTag("Player"))
            {
                obj.GetComponent<PlayerController>().InflictDamage(touchDamage);
            }
        }
    }
}


using System;
using Actor.AI.States;
using Actor.Interface;
using Actor.Player;
using Items;
using UI.Controllers;
using UnityEngine;
using UnityEngine.AI;
using Managers;
using Util.EditorHelpers;

namespace Actor.AI
{
    // General controller class for enemy AI. Scripts inheriting from AIState should be added to each enemy to create behavior.
    public class AIController : MonoBehaviour, IDamageable
    {
        public int itemSpawnChance = 20;                    // percent chance of spawning a random item
        public float hitPoints = 100;
        public int updateInterval = 3;
        public int activeRangeSq = 1000;  // only run FixedUpdate if in range of player
        public AIState initialState;
        public ItemSpawner.DropStruct[] drops;
        [SerializeField, ReadOnly] private AIState state;   // current state

        [HideInInspector] public NavMeshAgent agent;

        private int _updateCount;
        private bool _alive;                                // is the player alive
        private HUDController _hudController;

        void Awake()
        {
            agent = GetComponentInChildren<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        
        void Start()
        {
            if (!FindObjectOfType<NavMeshSurface2d>())
            {
                Debug.LogWarning("Need to add NavMeshPrefab to map and bake to use enemy. Also add NavMeshModifier to Ground and Walls of the Grid.");
                _alive = false;
                Destroy(this);
                return;
            }
            
            SetState(initialState);

            _hudController = UIManager.GetHudController();
            
            _alive = true;
        }
        
        private void FixedUpdate()
        {
            if (GameManager.IsFrozen || !_alive || ++_updateCount % updateInterval != 0) return;

            if (activeRangeSq < (PlayerController.Position - agent.transform.position).sqrMagnitude) return;
            
            if (hitPoints <= 0) OnKilled();
            
            if (state) state.UpdateState();
        }

        public void InflictDamage(GameObject instigator, float damage, float knockBack = 0)
        {
            Debug.Log($"Enemy hit for {damage} damage");
            hitPoints -= damage;
            _hudController.ShowFloatingText(agent.transform.position, "Hp-" + damage, Color.red);
            SoundManager.EnemyHit();
        }
        
        void OnKilled()
        {
            _alive = false;
            
            ItemSpawner.SpawnRandom(agent.transform.position, drops);

            Destroy(gameObject); // for now 
            // TODO change to dead sprite / make body searchable? 
        }

        public void SetState(AIState newState)
        {
            if (state) state.EndState();
            state = newState;
            if (state) state.StartState();
        }
    }
}


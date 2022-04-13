using System;
using System.Collections.Generic;
using Actor.AI.States;
using Actor.Interface;
using Actor.Player;
using Items;
using UI.Controllers;
using UnityEngine;
using UnityEngine.AI;
using Managers;
using Util.EditorHelpers;
using Util.Helpers;
using static Util.Helpers.CalculationHelper;

namespace Actor.AI
{
    // General controller class for enemy AI. Scripts inheriting from AIState should be added to each enemy to create behavior.
    public class AIController : MonoBehaviour, IDamageable
    {
        public float hitPoints = 100;
        public int updateInterval = 3;
        public int activeRangeSq = 1000;  // only run FixedUpdate if in range of player
        public AIState initialState;
        public AIState onHit;                               // state to transition to if hit by player
        public ItemSpawner.DropStruct[] drops;              // item drop chances
        [SerializeField, ReadOnly] private AIState state;   // current state
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Animator animator;

        private int _updateCount = 1;
        private bool _alive;                                // is the player alive
        private Rigidbody2D _rb;
        private HUDController _hudController;
        private SpriteRenderer _spriteRenderer;
        private Transform _weaponTransform;
        private int _animatorIsMovingId;
        public Transform WeaponTransform => _weaponTransform ??=  gameObject.GetChildObjectWithName("Sprite").GetChildObjectWithName("Weapon").GetComponent<Transform>();
        
        void Awake()
        {
            GameObject actor = gameObject.GetChildObjectWithName("Sprite");
            agent = actor.GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            _rb = actor.GetComponent<Rigidbody2D>();
            _spriteRenderer = actor.GetComponent<SpriteRenderer>();
            animator = actor.GetComponent<Animator>();
            _animatorIsMovingId = Animator.StringToHash("IsMoving");

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

            var velocity = agent.velocity;
            _spriteRenderer.flipX = velocity.x < 0;
            
            animator.SetBool(_animatorIsMovingId, velocity != Vector3.zero);
            if (agent.enabled) state.UpdateState();
            else if (_rb.velocity.sqrMagnitude < 1) EnableNavigation();
        }
        
        void OnKilled()
        {
            _alive = false;
            
            ItemSpawner.SpawnRandom(agent.transform.position, drops);
            
            // lay down and disable collision
            _rb.simulated = false;
            agent.GetComponent<Animator>().enabled = false;
            agent.transform.Rotate(new Vector3(0,0,1), 90);
            agent.GetComponent<SpriteRenderer>().color = new Color(0.2f,0.2f,0.2f,1);
            
        }

        public void SetState(AIState newState)
        {
            if (state) state.EndState();
            state = newState;
            if (state) state.StartState();
        }

        public void InflictDamage(float damage, float direction, float knockBack = 0) => InflictDamage(damage, direction.ToVector(), knockBack);
        
        public void InflictDamage(float damage, GameObject instigator, float knockBack = 0) => 
            InflictDamage(damage, (transform.position - instigator.transform.position), knockBack);
        
        public void InflictDamage(float damage, Vector2 direction, float knockBack = 0)
        {
            if (!_alive) return;
            
            Debug.Log($"Enemy hit for {damage} damage");
            
            _hudController.ShowFloatingText(agent.transform.position, "Hp-" + damage, Color.red);
            
            SoundManager.EnemyHit();
            
            hitPoints -= damage;
            
            if (hitPoints <= 0) OnKilled();

            else if (knockBack != 0)
            {
                DisableNavigation();
                _rb.AddForce(direction.normalized * knockBack, ForceMode2D.Impulse);
            }
        }

        public void DisableNavigation()
        {
            agent.enabled = false;
            _rb.isKinematic = false;
        }
        
        public void EnableNavigation()
        {
            agent.enabled = true;
            _rb.isKinematic = true;
            agent.nextPosition = _rb.position;
            SetState(onHit);
        }
    }
}


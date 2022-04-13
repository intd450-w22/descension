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
        
        public GameObject Actor => _actor ??= gameObject.GetChildObject("Sprite");
        public Transform Transform => _transform ??= Actor.transform;
        public Vector3 Position => Transform.position;
        public Rigidbody2D RigidBody => _rigidBody ??= Actor.GetComponent<Rigidbody2D>();
        public Collider2D Collider => _collider ??= Actor.GetComponent<Collider2D>();
        public NavMeshAgent Agent => _agent ??= Actor.GetComponent<NavMeshAgent>();
        public Animator Animator => _animator ??= Actor.GetComponent<Animator>();
        public Transform WeaponTransform => _weaponTransform ??= Actor.GetChildObject("Weapon").GetComponent<Transform>();

        private GameObject _actor;
        private Transform _transform;
        private Rigidbody2D _rigidBody;
        private Collider2D _collider;
        private NavMeshAgent _agent;
        private Animator _animator;
        private Transform _weaponTransform;
        
        // state
        private bool _alive;
        private int _updateCount = 1;
        private int _animatorIsMovingId;
        private bool _colliderIsTrigger;
        private HUDController _hudController;
        private SpriteRenderer _spriteRenderer;

        private bool _knocked;

        private bool knocked
        {
            get => _knocked;
            set
            {
                _knocked = value;
                if (_knocked)
                {
                    Agent.enabled = false;
                    RigidBody.isKinematic = false;
                    Collider.isTrigger = false;
                }

                else if (!_alive)
                {
                    RigidBody.simulated = false;
                }

                else
                {
                    Agent.enabled = true;
                    RigidBody.isKinematic = true;
                    Agent.nextPosition = RigidBody.position;
                    Collider.isTrigger = _colliderIsTrigger;
                    SetState(onHit);
                }
            }
        }
        
        void Awake()
        {
            Agent.updateRotation = false; 
            Agent.updateUpAxis = false;

            _spriteRenderer = Actor.GetComponent<SpriteRenderer>();
            _hudController = UIManager.GetHudController();
            _animatorIsMovingId = Animator.StringToHash("IsMoving");
            _colliderIsTrigger = Collider.isTrigger;
            
            _alive = true;
            
            SetState(initialState);
        }
        
        private void FixedUpdate()
        {
            if (GameManager.IsFrozen || !_alive || ++_updateCount % updateInterval != 0) return;

            if (activeRangeSq < (PlayerController.Position - Agent.transform.position).sqrMagnitude) return;

            var velocity = Agent.velocity;
            _spriteRenderer.flipX = velocity.x < 0;
            Animator.SetBool(_animatorIsMovingId, velocity != Vector3.zero);
            
            if (!knocked) state.UpdateState();
            else if (RigidBody.velocity.sqrMagnitude < 1) knocked = false;
        }
        
        void OnKilled()
        {
            _alive = false;
            
            ItemSpawner.SpawnRandom(Agent.transform.position, drops);
            
            // lay down and disable collision
            Animator.enabled = false;
            Agent.enabled = false;
            Transform.Rotate(new Vector3(0,0,1), 90);
            _spriteRenderer.color = new Color(0.2f,0.2f,0.2f,1);
        }

        public void SetState(AIState newState)
        {
            if (state) state.EndState();
            state = newState;
            if (state) state.StartState();
        }

        public void InflictDamage(float damage, float direction, float knockBack = 0) => 
            InflictDamage(damage, direction.ToVector(), knockBack);
        
        public void InflictDamage(float damage, GameObject instigator, float knockBack = 0) => 
            InflictDamage(damage, (transform.position - instigator.transform.position), knockBack);
        
        public void InflictDamage(float damage, Vector2 direction, float knockBack = 0)
        {
            if (!_alive) return;
            
            Debug.Log($"Enemy hit for {damage} damage");
            
            _hudController.ShowFloatingText(Agent.transform.position, "Hp-" + damage, Color.red);
            
            SoundManager.EnemyHit();
            
            hitPoints -= damage;
            
            if (hitPoints <= 0) OnKilled();

            if (knockBack != 0) KnockBack(direction.normalized * knockBack);
        }

        private void KnockBack(Vector2 forceVector)
        {
            knocked = true;
            RigidBody.AddForce(forceVector, ForceMode2D.Impulse);
        }
    }
}


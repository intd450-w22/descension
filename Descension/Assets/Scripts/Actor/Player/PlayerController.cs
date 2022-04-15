using System;
using System.Collections.Generic;
using System.Linq;
using Actor.Interface;
using Managers;
using UI.Controllers;
using Util.AssetMenu;
using Util.Enums;
using Util.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using Environment;

namespace Actor.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        // static accessors
        public static PlayerController Instance;
        private static Transform _transform;
        public static Transform SpriteTransform => _transform ??= Instance.gameObject.GetChildTransform("Sprite");
        public static Transform Reticle => Instance._reticle;
        public static Vector3 Position => Instance.transform.position;
        public static Transform ItemObject => Instance._itemObject;
        public static Sprite ItemSprite { set => Instance._itemSpriteRenderer.sprite = value; }
        public static Camera Camera => Instance._camera ??= Camera.main;
        public static Vector2 Velocity => Instance._rb.velocity;
        public static void SetStartPosition(int startPosition) => Instance._startPosition = startPosition;

        [Header("Configuration")]
        public DeviceDisplayConfigurator DeviceDisplaySettings;

        [Header("Attributes")]
        public float movementSpeed = 10;
        public float maxHitPoints = 100f;
        public float hitPoints = 100f;
        
        [Header("Torch")]
        public float flickerSpeed = 10;
        public float flickerMagnitude = 0.02f;
        public float TorchVignetteIntensityOn = 0.5f;
        public float TorchVignetteIntensityOff = 0.9f;

        [Header("Inventory")]
        public float ropeQuantity = 0;
        public float torchQuantity = 0;

        [Header("Scene Elements")] 
        public bool useUI = true;

        //Used for permanent death if die after bomb planted
        public FactKey EndFact;
        private Action _endGame;

        // Player input variables
        [HideInInspector] public PlayerInput playerInput;
        [HideInInspector] public PlayerControls playerControls;
        private string _currentControlScheme = ControlScheme.Desktop.ToString();

        // State variable 
        private Vector2 _rawInputMovement;

        // Components and GameObjects
        private HUDController _hudController;
        private Transform _reticle;
        private Transform _itemObject;
        private SpriteRenderer _itemSpriteRenderer;
        private Rigidbody2D _rb;
        private bool _torchToggle;
        private bool _torchIlluminated;  // prevents float comparison every frame
        private float _torchState = 0.9f;
        private postProcessingScript _postProcessing;
        private Animator _animator;
        private int _animatorIsMovingId;
        private SpriteRenderer _spriteRenderer;
        private bool _alive;
        private bool _knocked;
        private Camera _camera;
        [SerializeField] private int _startPosition;
        
        private Dictionary<int, AInteractable> _interactablesInRange = new Dictionary<int, AInteractable>();

        private bool knocked
        {
            get => _knocked;
            set { _knocked = value; _animator.enabled = !value; }
        }
        
        private bool alive
        {
            get => _alive;
            set { _alive = value; _animator.enabled = value; }
        }

        void Awake()
        {
            GameDebug.Log("Awake()");
            if (Instance == null)
            {
                Instance = this;
                
                _reticle = gameObject.GetChildTransform("Reticle");
                _reticle.gameObject.SetActive(false);
                _itemObject = gameObject.GetChildObject("Sprite").GetChildObject("Item").GetComponent<Transform>();
                _itemObject.gameObject.SetActive(false);
                _itemSpriteRenderer = _itemObject.GetComponent<SpriteRenderer>();
                _postProcessing = FindObjectOfType<postProcessingScript>();
                
                playerInput = GetComponent<PlayerInput>();
                playerControls = new PlayerControls();
                _animator = GetComponentInChildren<Animator>();
                _animatorIsMovingId = Animator.StringToHash("IsMoving");
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

                _rb = GetComponent<Rigidbody2D>();
                
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            Instance.GoToStartPosition();
            Instance.SetAlive();
            GameManager.Resume();
        }

        void GoToStartPosition()
        {
            var startPositions = GameObject.Find("StartPositions")?.GetChildTransforms().ToArray();
            if (startPositions?.Length > _startPosition)
            {
                Instance.transform.position = startPositions[_startPosition].position;
            }
        }

        void Start() => _hudController = UIManager.GetHudController();

        private void OnEnable() => playerControls?.Enable();
        
        private void OnDisable() => playerControls?.Disable();

        public static void Enable() => Instance.gameObject.Enable();
        public static void Disable() => Instance.gameObject.Disable();
        public static void ResetState()
        {
            Instance.hitPoints = Instance.maxHitPoints;
            ClearInteractablesInRange();
        }
        
        public static void OnReloadScene() => ResetState();
        
        public static void OnSceneComplete() => ClearInteractablesInRange();
        
        void FixedUpdate()
        {
            UpdateTorchVisuals();

            if (GameManager.IsFrozen)
            {
                _animator.SetBool(_animatorIsMovingId, false);
                return;
            }

            _hudController.UpdateUi(InventoryManager.Gold, ropeQuantity, torchQuantity, hitPoints, maxHitPoints);

            if (!knocked)
            {
                _rb.MovePosition(_rb.position + _rawInputMovement * movementSpeed);
                _animator.SetBool(_animatorIsMovingId, _rawInputMovement != Vector2.zero);
                _spriteRenderer.flipX = _rawInputMovement.x < 0 || (_spriteRenderer.flipX && _rawInputMovement.x == 0f);
            }
            else if (_rb.velocity.sqrMagnitude < 4) knocked = false;
        }

        private void UpdateTorchVisuals()
        {
            if (_torchToggle) 
            {
                if (torchQuantity > 0) 
                {
                    torchQuantity -= Time.deltaTime;
                    if (!_torchIlluminated && _torchState > TorchVignetteIntensityOn) _torchState -= TorchVignetteIntensityOn;
                    else _torchIlluminated = true;
                } 
                else _torchToggle = false;
            }
            else
            {
                if (_torchIlluminated && _torchState < TorchVignetteIntensityOff) _torchState += TorchVignetteIntensityOn;
                else _torchIlluminated = false;
            }
            _postProcessing.SetVignetteIntensity(_torchState + (float) Math.Cos(Time.time * flickerSpeed) * flickerMagnitude);
        }

        #region Entity Interaction


        public void InflictDamage(float damage, float direction, float knockBack = 0) => 
            InflictDamage(damage, direction.ToVector(), knockBack);
        
        public void InflictDamage(float damage, GameObject instigator, float knockBack = 0) => 
            InflictDamage(damage, (transform.position - instigator.transform.position).normalized, knockBack);

        public void InflictDamage(float damage, Vector2 direction, float knockBack = 0)
        {
            GameDebug.Log("InflictDamage(" + damage + ", " + direction + ", " + knockBack + ")");
            
            _hudController.ShowFloatingText(transform.position, "Hp-" + damage, Color.red);
            
            SoundManager.EnemyHit();
            
            hitPoints -= damage;
            
            if (hitPoints <= 0) OnKilled();

            if (knockBack != 0) KnockBack(direction.normalized * knockBack);
        }

        private void KnockBack(Vector2 forceVector)
        {
            knocked = true;
            _rb.AddForce(forceVector, ForceMode2D.Impulse);
        }
        
        private void SetAlive()
        {
            alive = true;
            gameObject.GetChildObject("Sprite").transform.rotation = new Quaternion{ eulerAngles = Vector3.zero };
            _spriteRenderer.color = Color.white;
        }
        
        private void SetDead()
        {
            alive = false;
            _spriteRenderer.color = new Color(0.2f,0.2f,0.2f,1);
            gameObject.GetChildObject("Sprite").transform.rotation = new Quaternion{ eulerAngles = new Vector3(0,0,-90) };
            InventoryManager.OnKilled();
        }
        
        public void HealDamage(float heal)
        {
            float healthRestored = Mathf.Min(maxHitPoints-hitPoints,heal);
            hitPoints += healthRestored;
            _hudController.ShowFloatingText(transform.position, "HP +" + healthRestored, Color.green);
        }

        public void OnKilled()
        {
            if (!alive || GameManager.IsFrozen) return;
            
            GameManager.Freeze();
            SetDead();
            if (FactManager.IsFactTrue(EndFact))
            {
                _endGame += PermanentDeath;
                GameManager.UnFreeze();
                DialogueManager.StartDialogue("", new[] { "The last adventurer to enter the Descent shall be remembered as a hero. Their sacrifice will always be remembered by those who will never have to suffer." }, _endGame);

            }
            else
            {
                Invoke(nameof(OpenDeathMenu), 3);
            }
        }

        void PermanentDeath()
        {
            GameManager.Pause();
            UIManager.SwitchUi(UIType.End);
        }

        public void OpenDeathMenu()
        {
            GameManager.UnFreeze();
            InventoryManager.OnKilled();
            GameManager.Pause();
            UIManager.SwitchUi(UIType.Death);
        }
        
        public static void ClearInteractablesInRange() => Instance._interactablesInRange.Clear();
        
        public static void AddInteractableInRange(int instanceId, AInteractable interactable) => Instance._AddInteractableInRange(instanceId, interactable);
        private void _AddInteractableInRange(int instanceId, AInteractable interactable)
        {
            _interactablesInRange.Add(instanceId, interactable);

            var closest = GetClosestInteractable();
            DialogueManager.ShowPrompt(closest.GetPrompt());
        }

        public static void RemoveInteractableInRange(int instanceId) => Instance._RemoveInteractableInRange(instanceId);
        private void _RemoveInteractableInRange(int instanceId)
        {
            _interactablesInRange.Remove(instanceId);

            if (_interactablesInRange.Any())
            {
                var closest = GetClosestInteractable();
                DialogueManager.ShowPrompt(closest.GetPrompt());
            }
            else
                DialogueManager.HidePrompt();
        }

        private static AInteractable GetClosestInteractable() => Instance._GetClosestInteractable();
        private AInteractable _GetClosestInteractable()
        {
            var location = gameObject.transform.position;
            return _interactablesInRange
                .Select(x => x.Value)
                .OrderBy(x => CalculationHelper.DistanceSq(location, x.Location()))
                .FirstOrDefault();
        }

        #endregion

        #region Player Input Callbacks

        public void OnPause(InputAction.CallbackContext value)
        {
            if(!value.started) return;

            if (GameManager.IsPaused)
            {
                GameManager.Resume();
                UIManager.SwitchUi(UIType.GameHUD);  
            }
            else
            {
                GameManager.Pause();
                UIManager.SwitchUi(UIType.PauseMenu);
            }
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            _rawInputMovement = value.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext value)
        {
            if (!value.started || GameManager.IsFrozen) return;

            InventoryManager.TryExecute();
        }

        public void OnDisplayNextLine(InputAction.CallbackContext value)
        {
            if (!DialogueManager.IsInDialogue || !value.started || GameManager.IsFrozen) return;
            DialogueManager.DisplayNextLine();
        }

        public void OnTorchToggle(InputAction.CallbackContext value)
        {
            if (!value.started || GameManager.IsFrozen) return;

            if (torchQuantity > 0) {
                SoundManager.ToggleTorch();
                _torchToggle = !_torchToggle;
            }
        }

        public void OnCodex(InputAction.CallbackContext value)
        {
            if (GameManager.IsFrozen) return;

            GameManager.Pause();
            UIManager.SwitchUi(UIType.Codex);
        }

        public void OnPickup(InputAction.CallbackContext value)
        {
            if (!value.started || GameManager.IsFrozen) return;

            // TODO: If we want we can add this, but not high priority
        }

        public void OnInteract(InputAction.CallbackContext value)
        {
            if (!value.started || GameManager.IsFrozen || !_interactablesInRange.Any()) return;

            _GetClosestInteractable()?.Interact();
        }

        public void OnDropItem(InputAction.CallbackContext value)
        {
            if (!value.started || GameManager.IsFrozen) return;

            InventoryManager.DropCurrentSlot();
        }

        public void OnSwapItemNum(InputAction.CallbackContext value)
        {
            if (!value.started || GameManager.IsFrozen) return;

            var index = (int)  value.ReadValue<float>();
            if (index > 0)
                InventoryManager.TryEquipSlot(index - 1);
        }

        public void OnControlsChanged()
        {
            if (!playerInput) return;

            if (playerInput.currentControlScheme != _currentControlScheme)
            {
                _currentControlScheme = playerInput.currentControlScheme;
                
                var deviceName = DeviceDisplaySettings.GetDeviceName(playerInput);
                GameDebug.Log($"Current control scheme {deviceName}");

                RemoveAllBindingOverrides();
            }
        }

        public void OnDeviceLost()
        {
            string disconnectedName = DeviceDisplaySettings.GetDisconnectedName();
            GameDebug.Log($"Device lost: {disconnectedName}");
        }

        public void RemoveAllBindingOverrides() { }

        #endregion

        #region Item Accessors

        public static void AddRope(int value) => Instance._AddRope(value);
        void _AddRope(int value) => ropeQuantity += value;

        public static void AddTorch(int value) => Instance._AddTorch(value);
        void _AddTorch(float value) => torchQuantity += value;
        
        #endregion
        
    }
}

using System;
using Actor.Interface;
using Managers;
using UI.Controllers;
using Util.AssetMenu;
using Util.Enums;
using Util.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using Environment;
using Util.EditorHelpers;
using static Util.Helpers.CalculationHelper;


namespace Actor.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        // static accessors
        public static PlayerController Instance;
        public static Transform Reticle => Instance._reticle;
        public static Vector3 Position => Instance.transform.position;
        public static Transform ItemObject => Instance._itemObject;
        public static Sprite ItemSprite { set => Instance._itemSpriteRenderer.sprite = value; }
        private Camera _camera;
        public static Camera Camera => Instance._camera ??= Camera.main;
        
        public static void OnReloadScene() => Instance._OnReloadScene();
        void _OnReloadScene()
        {
            hitPoints = maxHitPoints;
        }

        [Header("Configuration")]
        public DeviceDisplayConfigurator DeviceDisplaySettings;

        [Header("Attributes")]
        public float movementSpeed = 10;
        public float maxHitPoints = 100f;
        public float hitPoints = 100f;
        
        [Header("Torch")]
        public float flickerSpeed = 10;
        public float flickerMagnitude = 0.02f;
        public float torchVignetteIntensityOn = 0.5f;
        public float torchVignetteIntensityOff = 0.9f;

        [Header("Inventory")]
        public float ropeQuantity = 0;
        public float torchQuantity = 0;

        [Header("Scene Elements")] 
        public bool useUI = true;
        
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
        private postProcessingScript PostProcessing => _postProcessing ??= FindObjectOfType<postProcessingScript>();
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private bool _knocked;

        // current scene for death
        private string scene;

        void Awake()
        {
            Debug.Log("Awake()");
            if (Instance == null)
            {
                Instance = this;
                
                _reticle = gameObject.GetChildTransformWithName("Reticle");
                _reticle.gameObject.SetActive(false);

                _itemObject = gameObject.GetChildTransformWithName("Item");
                _itemObject.gameObject.SetActive(false);
                _itemSpriteRenderer = _itemObject.GetComponent<SpriteRenderer>();

                playerInput = GetComponent<PlayerInput>();
                playerControls = new PlayerControls();
                _animator = GetComponentInChildren<Animator>();
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

                _rb = GetComponent<Rigidbody2D>();
                
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Instance.transform.position = transform.position;
                
                Destroy(gameObject);
            }
            
            GameManager.Resume();
        }

        void Start()
        {
            _hudController = UIManager.GetHudController();
        }

        private void OnEnable()
        {
            playerControls?.Enable();
        }

        //
        private void OnDisable()
        {
            playerControls?.Disable();
        }

        void Update() 
        {
            if (GameManager.IsFrozen) return;

            // TODO: Move this to an input listener
            if (Input.GetKeyDown(KeyCode.J))
            {
                GameManager.Pause();
                UIManager.SwitchUi(UIType.Codex);
                return;
            }

            // TODO: Move this to an input listener        
            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                OnTorchToggle();
            }
        }


        void FixedUpdate()
        {
            if (GameManager.IsFrozen)
            {
                _animator.SetBool("IsMoving", false);
                return;
            }

            if (useUI) _hudController.UpdateUi(InventoryManager.Gold, ropeQuantity, torchQuantity, hitPoints);

            if (!_knocked) _rb.MovePosition(_rb.position + _rawInputMovement * movementSpeed);
            else if (_rb.velocity.sqrMagnitude < 4) _knocked = false;

            _spriteRenderer.flipX = _rawInputMovement.x < 0 || (_spriteRenderer.flipX && _rawInputMovement.x == 0f);
            
            if (_torchToggle) 
            {
                if (torchQuantity > 0) 
                {
                    torchQuantity -= 1 * Time.deltaTime;
                    if (!_torchIlluminated && _torchState > torchVignetteIntensityOn) _torchState -= 0.05f;
                    else _torchIlluminated = true;
                } 
                else _torchToggle = false;
            }
            else
            {
                if (_torchIlluminated && _torchState < torchVignetteIntensityOff) _torchState += 0.05f;
                else _torchIlluminated = false;
            }
            PostProcessing.SettVignetteIntensity(_torchState + (float) Math.Cos(Time.time*flickerSpeed)*flickerMagnitude);
        }

        #region Entity Interaction


        public void InflictDamage(float damage, float direction, float knockBack = 0) => 
            InflictDamage(damage, direction.ToVector(), knockBack);
        
        public void InflictDamage(float damage, GameObject instigator, float knockBack = 0) => 
            InflictDamage(damage, (transform.position - instigator.transform.position).normalized, knockBack);

        public void InflictDamage(float damage, Vector2 direction, float knockBack = 0)
        {
            Debug.Log("InflictDamage(" + damage + ", " + direction + ", " + knockBack + ")");
            
            _hudController.ShowFloatingText(transform.position, "Hp-" + damage, Color.red);
            
            SoundManager.EnemyHit();
            
            hitPoints -= damage;
            
            if (hitPoints <= 0) OnKilled();

            if (knockBack != 0)
            {
                _knocked = true;
                _rb.AddForce(direction.normalized * knockBack, ForceMode2D.Impulse);
            }
        }

        


        public void HealDamage(float heal)
        {
            float healthRestored = Mathf.Min(maxHitPoints-hitPoints,heal);
            hitPoints += healthRestored;
            _hudController.ShowFloatingText(transform.position, "HP +" + healthRestored, Color.green);
        }

        void OnKilled()
        {
            InventoryManager.OnKilled();

            if (GameManager.IsFrozen) return;

            GameManager.Pause();
            
            UIManager.SwitchUi(UIType.Death);
        }

        #endregion

        #region Player Input Callbacks

        public void OnPause()
        {
            if (GameManager.IsPaused) return;

            GameManager.Pause();

            // Display menu 
            UIManager.SwitchUi(UIType.PauseMenu);
        }

        public void OnResume()
        {
            GameManager.Resume();
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            _rawInputMovement = value.ReadValue<Vector2>();
            _animator.SetBool("IsMoving", _rawInputMovement != Vector2.zero);
        }

        public void OnAttack(InputAction.CallbackContext value)
        {
            // TODO: Get the callback working 
        }

        public void OnSpace(InputAction.CallbackContext value)
        {
            // if(value.started)
            //     _hudController.HideDialogue();
        }

        void OnTorchToggle()
        {
            if (torchQuantity > 0) {
                _torchToggle = !_torchToggle;
            }
        }

        public void OnControlsChanged()
        {
            if (!playerInput) return;

            if (playerInput.currentControlScheme != _currentControlScheme)
            {
                _currentControlScheme = playerInput.currentControlScheme;
                
                var deviceName = DeviceDisplaySettings.GetDeviceName(playerInput);
                Debug.Log($"Current control scheme {deviceName}");

                RemoveAllBindingOverrides();
            }
        }

        public void OnDeviceLost()
        {
            string disconnectedName = DeviceDisplaySettings.GetDisconnectedName();
            Debug.Log($"Device lost: {disconnectedName}");
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

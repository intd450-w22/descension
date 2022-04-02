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
        public static Transform Reticle => Instance._reticle;
        public static Vector3 Position => Instance.transform.position;
        public static Transform ItemObject => Instance._itemObject;
        public static Sprite ItemSprite { set => Instance._itemSpriteRenderer.sprite = value; }
        
        // for singleton
        // public static void OnReset() => Instance._OnReset();
        //
        // void _OnReset()
        // {
        //     hitPoints = maxHitPoints;
        // }
        
        

        [Header("Configuration")]
        public DeviceDisplayConfigurator DeviceDisplaySettings;

        [Header("Attributes")]
        public float movementSpeed = 10;
        public float maxHitPoints = 100f;
        public float hitPoints = 100f;
        
        private bool _torchToggle = true;

        [Header("Inventory")]
        public float ropeQuantity = 0;
        public float torchQuantity = 0;

        [Header("Scene Elements")] 
        public bool useUI = true;

        [HideInInspector] public Camera playerCamera;

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
        private postProcessingScript _postProcessing;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        // current scene for death
        private string scene;

        void Awake()
        {
            Instance = this;
            
            // TODO should probably be a singleton, but causes bugs currently when loading new level
            // if (Instance == null) Instance = this;
            // else if (Instance != this)
            // {
            //     Instance.transform.position = transform.position;
            //     Destroy(gameObject);
            // }
            //
            // DontDestroyOnLoad(gameObject);

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
        }

        void Start()
        {
            playerCamera = Camera.main;
            _hudController = UIManager.GetHudController();
            _postProcessing = FindObjectOfType<postProcessingScript>();

            GameManager.Resume();
        }

        private void OnEnable() => playerControls.Enable();

        private void OnDisable() => playerControls.Disable();

        void Update() {
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


        void FixedUpdate() {
            if (GameManager.IsFrozen)
            {
                _animator.SetBool("IsMoving", false);
                return;
            }

            if (useUI) _hudController.UpdateUi(InventoryManager.Gold, ropeQuantity, torchQuantity, hitPoints);

            _rb.MovePosition(_rb.position + _rawInputMovement * movementSpeed);
            _spriteRenderer.flipX = _rawInputMovement.x < 0 || (_spriteRenderer.flipX && _rawInputMovement.x == 0f);

            // TODO: Refactor to use a constant or variable instead of magic numbers
            if (_torchToggle) {
                if (torchQuantity > 0) {
                    torchQuantity -= 1 * Time.deltaTime;
                    _postProcessing?.SettVignetteIntensity(0.5f);
                } else {
                    _postProcessing?.SettVignetteIntensity(0.9f);
                }
            } else {
                _postProcessing?.SettVignetteIntensity(0.9f);
            }
        }

        #region Entity Interaction


        public static void InflictDamageStatic(GameObject instigator, float damage, float knockBack = 0) => Instance.InflictDamage(instigator, damage, knockBack);
        public void InflictDamage(GameObject instigator, float damage, float knockBack = 0) 
        {
            hitPoints -= damage;
            _hudController.ShowFloatingText(transform.position, "HP -" + damage, Color.red);

            if (knockBack != 0)
            {
                Vector2 direction = (transform.position - instigator.transform.position).normalized;
                _rb.AddForce(direction * knockBack);
            }

            if (hitPoints < 1)
            {
                OnKilled();
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
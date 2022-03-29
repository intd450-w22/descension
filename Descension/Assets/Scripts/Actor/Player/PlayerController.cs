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
        [Header("Configuration")]
        public DeviceDisplayConfigurator DeviceDisplaySettings;

        [Header("Attributes")]
        public float movementSpeed = 10;
        public float maxHitPoints = 100f;
        public float hitPoints = 100f;
        
        public float swordDamage = 25f; // obsolete -> moved to inventory item
        public float bowReticleDistance = 2f; // obsolete -> moved to inventory item
        public float swordReticleDistance = 1.5f; // obsolete -> moved to inventory item
        private bool _torchToggle = true;

        [Header("Session Variables")]
        // TODO: Change this to a "currWeapon" type thing 
        public bool hasBow = false;
        public bool hasSword = false;

        [Header("Inventory")]
        public float pickQuantity = 0; // obsolete -> moved to inventory item 
        public float arrowsQuantity = 0; // obsolete -> moved to inventory item
        public float ropeQuantity = 0;
        public float torchQuantity = 0;

        [Header("Scene Elements")] 
        public bool useUI = true;
        public GameObject arrowPrefab;
        
        [HideInInspector] public Camera playerCamera;

        // Player input variables
        [HideInInspector] public PlayerInput playerInput;
        [HideInInspector] public PlayerControls playerControls;
        private string _currentControlScheme = ControlScheme.Desktop.ToString();

        // State variable 
        private Vector2 _rawInputMovement;
        public bool isAttack; // obsolete -> sort of moved to items, can be refactored a lil bit 

        // Components and GameObjects
        private HUDController _hudController;
        private Transform _reticle;
        private Rigidbody2D _rb;
        private postProcessingScript _postProcessing;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        // current scene for death
        private string scene;

        void Awake() {
            _reticle = gameObject.GetChildTransformWithName("Reticle");
            if (_reticle != null && !hasBow && !hasSword)
                _reticle.gameObject.SetActive(false);

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

            // TODO: Find a better way to ensure game is started
            GameManager.IsPaused = false;
        }

        private void OnEnable() => playerControls.Enable();

        private void OnDisable() => playerControls.Disable();

        void Update() {
            // TODO: Move this to an input listener        
            if (Input.GetKeyDown(KeyCode.Q)) {
                OnTorchToggle();
            }
         }


        void FixedUpdate() {
            if (GameManager.IsPaused) return;

            if (useUI) _hudController.UpdateUi(InventoryManager.Gold, pickQuantity, arrowsQuantity, ropeQuantity, torchQuantity, hitPoints);

            _rb.MovePosition(_rb.position + _rawInputMovement * movementSpeed);
            _spriteRenderer.flipX = _rawInputMovement.x < 0 || (_spriteRenderer.flipX && _rawInputMovement.x == 0f);

            // TODO: Refactor to use a constant or variable instead of magic numbers
            if (_torchToggle) {
                if (torchQuantity > 0) {
                    torchQuantity -= 1 * Time.deltaTime;
                    _postProcessing.SettVignetteIntensity(0.5f);
                } else {
                    _postProcessing.SettVignetteIntensity(0.9f);
                }
            } else {
                _postProcessing.SettVignetteIntensity(0.9f);
            }
        }

        #region Entity Interaction

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

        public void OnKilled()
        {
            InventoryManager.OnKilled();

            if (GameManager.IsPaused) return;

            GameManager.IsPaused = true;
            
            UIManager.SwitchUi(UIType.Death);
        }

        #endregion

        #region Player Input Callbacks

        public void OnPause()
        {
            if (GameManager.IsPaused) return;

            GameManager.IsPaused = true;

            // Display menu 
            UIManager.SwitchUi(UIType.PauseMenu);
        }

        public void OnResume()
        {
            GameManager.IsPaused = false;
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
            if(value.started)
                _hudController.HideDialogue();
        }

        public void OnTorchToggle()
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

        // obsolete -> moved to inventory item
        public void AddPick(float value) => pickQuantity += value;

        // obsolete -> moved to inventory item
        public void AddBow()
        {
            hasBow = true;
            if (_reticle != null)
                _reticle.gameObject.SetActive(true);
        }

        // obsolete -> moved to inventory item
        public void AddSword() => hasSword = true;

        // obsolete -> moved to inventory item
        public void AddArrows(float value) => arrowsQuantity += value;

        public void AddRope(float value) => ropeQuantity += value;

        public void AddTorch(float value) => torchQuantity += value;
        
        #endregion

    }
}
using System;
using System.Collections.Generic;
using Actor.AI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Util.AssetMenu;
using Util.Enums;
using Util.Helpers;
using Util.Structs;

namespace Actor.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Configuration")]
        public DeviceDisplayConfigurator DeviceDisplaySettings;

        [Header("Attributes")]
        public float movementSpeed = 10;
        public float hitPoints = 100f;
        public float score = 0f; // TODO: Should belong to a "game manager" 
        public float swordDamage = 25f;
        public float bowReticleDistance = 2f;
        public float swordReticleDistance = 1.5f;

        [Header("Session Variables")]
        // TODO: Change this to a "currWeapon" type thing 
        public bool hasBow = false;
        public bool hasSword = false;

        [Header("Inventory")]
        public float pickQuantity = 0;
        public float arrowsQuantity = 0;
        public float ropeQuantity = 0;
        public float torchQuantity = 0;

        // TODO: These should not be injected via the editor. We can 
        // TODO: use something like tags to identify them in the scene
        // TODO: and assign references. 
        [Header("Scene Elements")] 
        public bool useUI = true;
        public Image dialogueBox;
        public Text dialogueText;
        public Text scoreUI;
        public Text bowUI;
        public Text pickUI;
        public Text torchUI;
        public Text ropeUI;
        public GameObject floatingTextDamage;
        public GameObject arrowPrefab;
        
        private Camera _playerCamera;

        // Player input variables
        private PlayerInput _playerInput;
        private PlayerControls _playerControls;
        private string _currentControlScheme = ControlScheme.Desktop.ToString();

        // State variable 
        private Vector2 _rawInputMovement;
        private bool _isAttack;

        // Components and GameObjects
        private Transform _reticle;
        private Rigidbody2D _rb;

        void Awake() {
            // TODO: These work here for now, but should be moved later.
            if(dialogueBox != null) dialogueBox.enabled = false;
            if(dialogueText != null) dialogueText.enabled = false;
            if(scoreUI != null) scoreUI.enabled = true;
            if(bowUI != null) bowUI.enabled = false;
            if(pickUI != null) pickUI.enabled = false;
            if(torchUI != null) torchUI.enabled = false;
            if(ropeUI != null) ropeUI.enabled = false;

            _reticle = gameObject.GetChildTransformWithName("Reticle");
            if (_reticle != null && !hasBow)
                _reticle.gameObject.SetActive(false);

            _playerCamera = Camera.main;

            _playerInput = GetComponent<PlayerInput>();
            _playerControls = new PlayerControls();

            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable() => _playerControls.Enable();

        private void OnDisable() => _playerControls.Disable();

        void Update() {
            if(useUI) UpdateUi();

            _rb.velocity = _rawInputMovement * movementSpeed;

            if(useUI)
                // what does this do ? 
                if ((dialogueBox.enabled || dialogueText.enabled) && Input.GetKeyDown(KeyCode.Space)) {
                    dialogueBox.enabled = false;
                    dialogueText.enabled = false;
                }

            if (torchQuantity > 0) {
                torchQuantity -= 2 * Time.deltaTime;
            }

            // TODO: Remove this once the callback is functioning
            _isAttack = _playerControls.Default.Shoot.WasPerformedThisFrame();
            if (hasBow)
            {
                var screenPoint = _playerCamera.WorldToScreenPoint(transform.localPosition);
                var direction = (Input.mousePosition - screenPoint).normalized;

                if (_reticle != null)
                {
                    _reticle.gameObject.SetActive(true);

                    // Set the position of the reticle on the screen according to input type
                    if (_currentControlScheme == ControlScheme.Desktop.ToString())
                    {
                        // Place the reticle on the cursor 
                        // TODO: Hide the cursor ? 
                        _reticle.position = (Vector2) _playerCamera.ScreenToWorldPoint(Input.mousePosition);
                    }
                    else if (_currentControlScheme == ControlScheme.Gamepad.ToString())
                    {
                        // Place the reticle in a ring around the player 
                        // TODO: Add aiming with the right stick ala Enter the Gungeon 
                        _reticle.position = transform.position + (direction * bowReticleDistance);
                    }
                }

                Debug.DrawLine(transform.position, transform.position + direction * 3);

                if (_isAttack && arrowsQuantity > 0) {
                
                    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
                    arrowsQuantity -= 1;
                }                
            }
            else if (hasSword)
            {
                var screenPoint = _playerCamera.WorldToScreenPoint(transform.localPosition);
                var direction = (Input.mousePosition - screenPoint).normalized;

                if (_reticle != null)
                {
                    _reticle.gameObject.SetActive(true);
                    _reticle.position = transform.position + (direction * swordReticleDistance);
                }

                Debug.DrawLine(transform.position, transform.position + direction * swordReticleDistance);

                if (_isAttack) {
                
                    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    var attackPoint = (Vector2) (transform.position + (direction * swordReticleDistance));
                    var hitEnemies = Physics2D.OverlapBoxAll(attackPoint, new Vector2(2, 2), angle, LayerMask.GetMask("Enemy"));
                    foreach (var enemy in hitEnemies)
                    {
                        try { enemy.gameObject.GetComponent<AIController>().InflictDamage(swordDamage); }
                        catch { enemy.gameObject.GetComponentInParent<AIController>().InflictDamage(swordDamage); }
                    }
                }
            }
            
        }

        #region Entity Interaction

        public void InflictDamage(float damage) {
            hitPoints -= damage;
            ShowFloatingTextDamage("HP -" + damage.ToString());
        }

        #endregion

        #region Player Input Callbacks

        public void OnMovement(InputAction.CallbackContext value)
        {
            _rawInputMovement = value.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext value)
        {
            // TODO: Get the callback working 
        }

        public void OnControlsChanged()
        {
            if (!_playerInput) return;

            if (_playerInput.currentControlScheme != _currentControlScheme)
            {
                _currentControlScheme = _playerInput.currentControlScheme;
                
                var deviceName = DeviceDisplaySettings.GetDeviceName(_playerInput);
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

        public void AddPick(float value) => pickQuantity += value;

        public void AddBow()
        {
            hasBow = true;
            if (_reticle != null)
                _reticle.gameObject.SetActive(true);
        }

        public void AddSword() => hasSword = true;

        public void AddArrows(float value) => arrowsQuantity += value;

        public void AddRope(float value) => ropeQuantity += value;

        public void AddTorch(float value) => torchQuantity += value;
        
        #endregion

        #region UI Controls

        private void ShowFloatingTextDamage(string text) {
            var t = Instantiate(floatingTextDamage, transform.position, Quaternion.identity);
            t.GetComponent<TextMesh>().text = text;
        }

        private void showText(string text) {
            dialogueBox.enabled = true;
            dialogueText.enabled = true;
            dialogueText.text = text;
        }

        private void UpdateUi()
        {
            scoreUI.text = "Gold/Score: " + score.ToString();

            if (pickQuantity > 0)
            {
                pickUI.enabled = true;
                pickUI.text = "Pick " + pickQuantity.ToString();
            }
            else
            {
                pickUI.enabled = false;
            }

            if (arrowsQuantity > 0)
            {
                bowUI.enabled = true;
                bowUI.text = "Arrows " + arrowsQuantity.ToString();
            }
            else
            {
                bowUI.enabled = false;
            }

            if (ropeQuantity > 0)
            {
                ropeUI.enabled = true;
                ropeUI.text = "Rope " + ropeQuantity.ToString();
            }
            else
            {
                ropeUI.enabled = false;
            }

            if (torchQuantity > 0)
            {
                torchUI.enabled = true;
                torchUI.text = "Torch " + Mathf.Floor(torchQuantity).ToString();
            }
            else
            {
                torchUI.enabled = false;
            }
        }

        #endregion

    }
}
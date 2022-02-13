using Actor.AI;
using UnityEngine;
using UnityEngine.UI;
using Util.Enums;
using Util.Helpers;

namespace Actor.Player
{
    public class PlayerController : MonoBehaviour
    {
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

        private PlayerControls _playerControls;

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

            _playerCamera = Camera.main;

            _playerControls = new PlayerControls();

            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable() => _playerControls.Enable();

        private void OnDisable() => _playerControls.Disable();

        void Update() {
            if(useUI) UpdateUi();

            var move = _playerControls.Default.Move.ReadValue<Vector2>();
            _rb.velocity = new Vector2(move.x * movementSpeed, move.y * movementSpeed);

            if(useUI)
                // what does this do ? 
                if ((dialogueBox.enabled || dialogueText.enabled) && Input.GetKeyDown(KeyCode.Space)) {
                    dialogueBox.enabled = false;
                    dialogueText.enabled = false;
                }

            if (torchQuantity > 0) {
                torchQuantity -= 2 * Time.deltaTime;
            }

            if (hasBow)
            {                
                var isShoot = _playerControls.Default.Shoot.WasPressedThisFrame();

                var screenPoint = _playerCamera.WorldToScreenPoint(transform.localPosition);
                var direction = (Input.mousePosition - screenPoint).normalized;

                if(_reticle != null)
                    _reticle.position = transform.position + (direction * bowReticleDistance);

                Debug.DrawLine(transform.position, transform.position + direction);

                if (isShoot && arrowsQuantity > 0) {
                
                    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
                    arrowsQuantity -= 1;
                }                
            }
            else if (hasSword)
            {
                var isAttack = _playerControls.Default.Shoot.WasPressedThisFrame();

                var screenPoint = _playerCamera.WorldToScreenPoint(transform.localPosition);
                var direction = (Input.mousePosition - screenPoint).normalized;

                if(_reticle != null)
                    _reticle.position = transform.position + (direction * swordReticleDistance);

                Debug.DrawLine(transform.position, transform.position + direction);

                if (isAttack) {
                
                    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    var attackPoint = (Vector2) (transform.position + (direction * swordReticleDistance));
                    var hitEnemies = Physics2D.OverlapBoxAll(attackPoint, new Vector2(1, 2), angle, LayerMask.GetMask("Enemy"));
                    foreach (var enemy in hitEnemies)
                    {
                        try { enemy.gameObject.GetComponent<AIController>().InflictDamage(swordDamage); }
                        catch { enemy.gameObject.GetComponentInParent<AIController>().InflictDamage(swordDamage); }
                    }
                }
            }
            
        }

        public void AddPick(float value) => pickQuantity += value;

        public void AddBow() => hasBow = true;

        public void AddSword() => hasSword = true;

        public void AddArrows(float value) => arrowsQuantity += value;

        public void AddRope(float value) => ropeQuantity += value;

        public void AddTorch(float value) => torchQuantity += value;

        public void InflictDamage(float damage) {
            hitPoints -= damage;
            ShowFloatingTextDamage("HP -" + damage.ToString());
        }

        private void ShowFloatingTextDamage(string text) {
            var t = Instantiate(floatingTextDamage, transform.position, Quaternion.identity);
            t.GetComponent<TextMesh>().text = text;
        }

        private void showText(string text) {
            dialogueBox.enabled = true;
            dialogueText.enabled = true;
            dialogueText.text = text;
        }

        private void UpdateUi() {
            scoreUI.text = "Gold/Score: " + score.ToString();

            if (pickQuantity > 0) {
                pickUI.enabled = true;
                pickUI.text = "Pick " + pickQuantity.ToString();
            } else {
                pickUI.enabled = false;
            }

            if (arrowsQuantity > 0) {
                bowUI.enabled = true;
                bowUI.text = "Arrows " + arrowsQuantity.ToString();
            } else {
                bowUI.enabled = false;
            }

            if (ropeQuantity > 0) {
                ropeUI.enabled = true;
                ropeUI.text = "Rope " + ropeQuantity.ToString();
            } else {
                ropeUI.enabled = false;
            }

            if (torchQuantity > 0) {
                torchUI.enabled = true;
                torchUI.text = "Torch " + Mathf.Floor(torchQuantity).ToString();
            } else {
                torchUI.enabled = false;
            }
        }
    }
}
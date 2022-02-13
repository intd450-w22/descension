using UnityEngine;
using UnityEngine.UI;

namespace Actor.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Attributes")]
        public float movementSpeed = 10;
        public float hitPoints = 100f;
        public float score = 0f; // TODO: Should belong to a "game manager" 

        [Header("Session Variables")]
        public bool hasBow = false;

        [Header("Inventory")]
        public float pickQuantity = 0;
        public float arrowsQuantity = 0;
        public float ropeQuantity = 0;
        public float torchQuantity = 0;

        // TODO: These should not be injected via the editor. We can 
        // TODO: use something like tags to identify them in the scene
        // TODO: and assign references. 
        [Header("Scene Elements")] 
        public bool UseUI = true;
        public Image dialogueBox;
        public Text dialogueText;
        public Text scoreUI;
        public Text bowUI;
        public Text pickUI;
        public Text torchUI;
        public Text ropeUI;
        public GameObject floatingTextDamage;
        public GameObject ArrowPrefab;
        
        private Camera _playerCamera;

        private PlayerControls _playerControls;

        void Awake() {
            // TODO: These work here for now, but should be moved later.
            if(dialogueBox != null) dialogueBox.enabled = false;
            if(dialogueText != null) dialogueText.enabled = false;
            if(scoreUI != null) scoreUI.enabled = true;
            if(bowUI != null) bowUI.enabled = false;
            if(pickUI != null) pickUI.enabled = false;
            if(torchUI != null) torchUI.enabled = false;
            if(ropeUI != null) ropeUI.enabled = false;

            _playerCamera = Camera.main;

            _playerControls = new PlayerControls();
        }

        private void OnEnable() => _playerControls.Enable();

        private void OnDisable() => _playerControls.Disable();

        void Update() {
            if(UseUI) updateUI();

            var move = _playerControls.Default.Move.ReadValue<Vector2>();
            if (move.x != 0 || move.y != 0) {
                transform.Translate(move.x * movementSpeed * Time.deltaTime,  move.y * movementSpeed * Time.deltaTime, 0);
            }

            if(UseUI)
                // what does this do ? 
                if ((dialogueBox.enabled || dialogueText.enabled) && Input.GetKeyDown(KeyCode.Space)) {
                    dialogueBox.enabled = false;
                    dialogueText.enabled = false;
                }

            if (torchQuantity > 0) {
                torchQuantity -= 2 * Time.deltaTime;
            }

            // shoot arrows if conditions are fulfilled
            var isShoot = _playerControls.Default.Shoot.WasPressedThisFrame();
            if (isShoot && hasBow && arrowsQuantity > 0) {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 screenPoint = _playerCamera.WorldToScreenPoint(transform.localPosition);
                Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
                float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                Instantiate(ArrowPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
                arrowsQuantity -= 1;
            }
        }

        public void AddPick(float value) => pickQuantity += value;

        public void AddBow() => hasBow = true;

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

        private void updateUI() {
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

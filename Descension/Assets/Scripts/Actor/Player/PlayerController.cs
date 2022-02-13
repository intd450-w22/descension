using UnityEngine;
using UnityEngine.UI;

namespace Actor.Player
{
    public class PlayerController : MonoBehaviour
    {
        // stats
        public float movementSpeed = 10;
        public float hitPoints = 100f;
        public float score = 0f;

        // key items
        public bool hasBow = false;

        // consumable items
        public float pickQuantity = 0;
        public float arrowsQuantity = 0;
        public float ropeQuantity = 0;
        public float torchQuantity = 0;

        public Image dialogueBox;
        public Text dialogueText;
        public Text scoreUI;
        public Text bowUI;
        public Text pickUI;
        public Text torchUI;
        public Text ropeUI;
        public GameObject floatingTextDamage;
        private Camera playerCamera;

        // projectile object in the game. create one every time the PlayerController shoots
        public GameObject arrow;
    
        void Awake() {
            // TODO: These work here for now, but should be moved later 
            dialogueBox.enabled = false;
            dialogueText.enabled = false;
            scoreUI.enabled = true;
            bowUI.enabled = false;
            pickUI.enabled = false;
            torchUI.enabled = false;
            ropeUI.enabled = false;

            playerCamera = Camera.main;
        }

        void Update() {
            this.updateUI();

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            if (x != 0 || y != 0) {
                transform.Translate(x * movementSpeed * Time.deltaTime,  y * movementSpeed * Time.deltaTime, 0);
            }

            if ((dialogueBox.enabled || dialogueText.enabled) && Input.GetKeyDown(KeyCode.Space)) {
                dialogueBox.enabled = false;
                dialogueText.enabled = false;
            }

            if (this.torchQuantity > 0) {
                this.torchQuantity -= 2 * Time.deltaTime;
            }

            // shots arrows if conditions are fulfilled
            if (Input.GetMouseButtonDown(0) && hasBow && arrowsQuantity > 0) {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 screenPoint = playerCamera.WorldToScreenPoint(transform.localPosition);
                Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
                float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                Instantiate(arrow, transform.position, Quaternion.Euler(0f, 0f, angle));
                this.arrowsQuantity -= 1;
            }

            if (Input.GetMouseButtonDown(0) && hasBow) {
                showText("No arrows to shoot");
            }
        }

        public void addPick(float value) {
            this.pickQuantity += value;
        }

        public void addBow() {
            this.hasBow = true;
        }

        public void addArrows(float value) {
            this.arrowsQuantity += value;
        }

        public void addRope(float value) {
            this.ropeQuantity += value;
        }

        public void addTorch(float value) {
            this.torchQuantity += value;
        }

        public void inflictDamage(float damage) {
            this.hitPoints -= damage;
            showFloatingTextDamage("HP -" + damage.ToString());
        }

        private void showFloatingTextDamage(string text) {
            var t = Instantiate(floatingTextDamage, transform.position, Quaternion.identity);
            t.GetComponent<TextMesh>().text = text;
        }

        private void showText(string text) {
            dialogueBox.enabled = true;
            dialogueText.enabled = true;
            dialogueText.text = text;
        }

        private void updateUI() {
            scoreUI.text = "Gold/Score: " + score.ToString();

            if (this.pickQuantity > 0) {
                pickUI.enabled = true;
                pickUI.text = "Pick " + this.pickQuantity.ToString();
            } else {
                pickUI.enabled = false;
            }

            if (this.arrowsQuantity > 0) {
                bowUI.enabled = true;
                bowUI.text = "Arrows " + this.arrowsQuantity.ToString();
            } else {
                bowUI.enabled = false;
            }

            if (this.ropeQuantity > 0) {
                ropeUI.enabled = true;
                ropeUI.text = "Rope " + this.ropeQuantity.ToString();
            } else {
                ropeUI.enabled = false;
            }

            if (this.torchQuantity > 0) {
                torchUI.enabled = true;
                torchUI.text = "Torch " + Mathf.Floor(this.torchQuantity).ToString();
            } else {
                torchUI.enabled = false;
            }
        }
    }
}

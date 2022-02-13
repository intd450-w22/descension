using UnityEngine;
using UnityEngine.UI;

namespace Actor.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Attributes")]
        public float MovementSpeed = 10;
        public float HitPoints = 100f;
        public float Score = 0f; // TODO: Should belong to a "game manager" 

        [Header("Session Variables")]
        public bool HasBow = false;

        [Header("Inventory")]
        public float PickQuantity = 0;
        public float ArrowsQuantity = 0;
        public float RopeQuantity = 0;
        public float TorchQuantity = 0;

        // TODO: These should not be injected via the editor. We can 
        // TODO: use something like tags to identify them in the scene
        // TODO: and assign references. 
        [Header("Scene Elements")] 
        public bool UseUi = true;
        public Image DialogueBox;
        public Text DialogueText;
        public Text ScoreUi;
        public Text BowUi;
        public Text PickUi;
        public Text TorchUi;
        public Text RopeUi;
        public GameObject FloatingTextDamage;
        public GameObject ArrowPrefab;
        
        private Camera _playerCamera;

        private PlayerControls _playerControls;

        void Awake() {
            // TODO: These work here for now, but should be moved later.
            if(DialogueBox != null) DialogueBox.enabled = false;
            if(DialogueText != null) DialogueText.enabled = false;
            if(ScoreUi != null) ScoreUi.enabled = true;
            if(BowUi != null) BowUi.enabled = false;
            if(PickUi != null) PickUi.enabled = false;
            if(TorchUi != null) TorchUi.enabled = false;
            if(RopeUi != null) RopeUi.enabled = false;

            _playerCamera = Camera.main;

            _playerControls = new PlayerControls();
        }

        private void OnEnable() => _playerControls.Enable();

        private void OnDisable() => _playerControls.Disable();

        void Update() {
            if(UseUi) UpdateUi();

            var move = _playerControls.Default.Move.ReadValue<Vector2>();
            if (move.x != 0 || move.y != 0) {
                transform.Translate(move.x * MovementSpeed * Time.deltaTime,  move.y * MovementSpeed * Time.deltaTime, 0);
            }

            if(UseUi)
                // what does this do ? 
                if ((DialogueBox.enabled || DialogueText.enabled) && Input.GetKeyDown(KeyCode.Space)) {
                    DialogueBox.enabled = false;
                    DialogueText.enabled = false;
                }

            if (TorchQuantity > 0) {
                TorchQuantity -= 2 * Time.deltaTime;
            }

            // shoot arrows if conditions are fulfilled
            var isShoot = _playerControls.Default.Shoot.WasPressedThisFrame();
            if (isShoot && HasBow && ArrowsQuantity > 0) {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 screenPoint = _playerCamera.WorldToScreenPoint(transform.localPosition);
                Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
                float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                Instantiate(ArrowPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
                ArrowsQuantity -= 1;
            }
        }

        public void AddPick(float value) => PickQuantity += value;

        public void AddBow() => HasBow = true;

        public void AddArrows(float value) => ArrowsQuantity += value;

        public void AddRope(float value) => RopeQuantity += value;

        public void AddTorch(float value) => TorchQuantity += value;

        public void InflictDamage(float damage) {
            HitPoints -= damage;
            ShowFloatingTextDamage("HP -" + damage.ToString());
        }

        private void ShowFloatingTextDamage(string text) {
            var t = Instantiate(FloatingTextDamage, transform.position, Quaternion.identity);
            t.GetComponent<TextMesh>().text = text;
        }

        private void UpdateUi() {
            ScoreUi.text = "Gold/Score: " + Score.ToString();

            if (PickQuantity > 0) {
                PickUi.enabled = true;
                PickUi.text = "Pick " + PickQuantity.ToString();
            } else {
                PickUi.enabled = false;
            }

            if (ArrowsQuantity > 0) {
                BowUi.enabled = true;
                BowUi.text = "Arrows " + ArrowsQuantity.ToString();
            } else {
                BowUi.enabled = false;
            }

            if (RopeQuantity > 0) {
                RopeUi.enabled = true;
                RopeUi.text = "Rope " + RopeQuantity.ToString();
            } else {
                RopeUi.enabled = false;
            }

            if (TorchQuantity > 0) {
                TorchUi.enabled = true;
                TorchUi.text = "Torch " + Mathf.Floor(TorchQuantity).ToString();
            } else {
                TorchUi.enabled = false;
            }
        }
    }
}

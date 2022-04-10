using Managers;
using TMPro;
using UI.MenuUI;
using UnityEngine;
using UnityEngine.UI;
using Util.Helpers;

namespace UI.Controllers
{
    public class HUDController : MonoBehaviour
    {
        [Header("UI Prefabs")]
        public GameObject FloatingTextDamagePrefab;
        public GameObject FloatingTextDialoguePrefab;

        private TextMeshProUGUI _promptText;
        private Image _dialogueBox;
        private TextMeshProUGUI _dialogueName;
        private TextMeshProUGUI _dialogueText;
        private Button _continueButton;
        private TextMeshProUGUI _continueButtonText;
        private TextMeshProUGUI _goldUI;
        private TextMeshProUGUI _torchUI;
        private TextMeshProUGUI _ropeUI;
        private TextMeshProUGUI _healthUI;
        private ProgressBar _healthBar;
        private Hotbar _hotbar;

        public Hotbar Hotbar { get => _hotbar; }

        void Awake()
        {
            Init();
        }

        public void Init()
        {
            SetReferences();
            Reset();
        }

        public void SetReferences()
        {
            try
            {
                _promptText = gameObject.GetChildObjectWithName("NotificationText").GetComponent<TextMeshProUGUI>();
                _dialogueBox = gameObject.GetChildObjectWithName("DialogueBox").GetComponent<Image>();
                _dialogueName = _dialogueBox.gameObject.GetChildObjectWithName("DialogueBoxName").GetComponent<TextMeshProUGUI>();
                _dialogueText = _dialogueBox.gameObject.GetChildObjectWithName("DialogueBoxText").GetComponent<TextMeshProUGUI>();
                _continueButton = _dialogueBox.gameObject.GetChildObjectWithName("ContinueButton").GetComponent<Button>();
                _continueButtonText = _continueButton.gameObject.GetChildObjectWithName("ContinueButtonText").GetComponent<TextMeshProUGUI>();
                
                var rightHudGroup = gameObject.GetChildObjectWithName("RightHudGroup").gameObject;
                _goldUI = rightHudGroup.GetChildObjectWithName("Gold").GetComponent<TextMeshProUGUI>();
                _torchUI = rightHudGroup.GetChildObjectWithName("TorchDurability").GetComponent<TextMeshProUGUI>();
                _ropeUI = rightHudGroup.GetChildObjectWithName("RopeDurability").GetComponent<TextMeshProUGUI>();

                var leftHudGroup = gameObject.GetChildObjectWithName("LeftHudGroup");
                _healthUI = leftHudGroup.GetChildObjectWithName("Health").GetComponent<TextMeshProUGUI>();
                _healthBar = leftHudGroup.GetChildObjectWithName("HealthBar").GetComponent<ProgressBar>();

                _hotbar = GetComponentInChildren<Hotbar>();
            }
            catch
            {
                // ignored
            }
        }

        public void Reset()
        {
            try
            {
                _promptText.enabled = false;
                _dialogueBox.enabled = false;
                _dialogueName.enabled = false;
                _dialogueText.enabled = false;
                _continueButton.enabled = false;
                _continueButtonText.enabled = false;
                _goldUI.enabled = true;
                _torchUI.enabled = false;
                _ropeUI.enabled = false;
                _healthUI.enabled = true;
                _healthBar.enabled = true;
                _hotbar.enabled = true;

                _healthBar.Value = _healthBar.Max;
            }
            catch
            {
                // ignored
            }
        }

        public void HideDialogue()
        {
            _promptText.enabled = false;
            _dialogueBox.enabled = false;
            _dialogueName.enabled = false;
            _dialogueText.enabled = false;
            _continueButton.enabled = false;
            _continueButtonText.enabled = false;
        }

        public void ShowFloatingText(Vector2 location, string text, Color? color = null) => ShowFloatingText((Vector3) location, text, color);

        public void ShowFloatingText(Vector3 location, string text, Color? color = null) {
            var t = Instantiate(FloatingTextDamagePrefab, location, Quaternion.identity).GetComponent<TextMesh>();
            t.text = text;
            t.color = color ?? Color.black;
        }

        public void ShowText(string text, string name = "") {
            _promptText.enabled = false;
            _dialogueBox.enabled = true;
            _dialogueName.enabled = true;
            _dialogueText.enabled = true;
            _continueButton.enabled = true;
            _continueButtonText.enabled = true;

            _dialogueName.text = name;
            _dialogueText.text = text;
        }

        public void ShowPrompt(string text) {
            _promptText.enabled = true;
            _promptText.text = text;
        }

        public void UpdateUi(float gold, float ropeQuantity, float torchQuantity, float health)
        {
            try
            {
                _goldUI.text = $"Gold: {gold}";
                _healthUI.text = $"Health: {health}";
                _healthBar.Value = health;

                if (ropeQuantity > 0)
                {
                    _ropeUI.enabled = true;
                    _ropeUI.text = "Rope " + ropeQuantity;
                }
                else
                {
                    _ropeUI.enabled = false;
                }

                if (torchQuantity > 0)
                {
                    _torchUI.enabled = true;
                    _torchUI.text = "Torch " + Mathf.Floor(torchQuantity);
                }
                else
                {
                    _torchUI.enabled = false;
                }
            }
            catch (MissingReferenceException e)
            {
                Debug.LogWarning(e.Message);
                UIManager.ReinitHudController();
            }
            
        }
    }
}

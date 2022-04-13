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
        private GameObject _ropeGroup;
        private TextMeshProUGUI _ropeUI;
        private GameObject _torchGroup;
        private TextMeshProUGUI _torchUI;
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
                _promptText = gameObject.GetChildObjectWithName("PromptText").GetComponent<TextMeshProUGUI>();
                _dialogueBox = gameObject.GetChildObjectWithName("DialogueBox").GetComponent<Image>();
                _dialogueName = _dialogueBox.gameObject.GetChildObjectWithName("DialogueBoxName").GetComponent<TextMeshProUGUI>();
                _dialogueText = _dialogueBox.gameObject.GetChildObjectWithName("DialogueBoxText").GetComponent<TextMeshProUGUI>();
                _continueButton = _dialogueBox.gameObject.GetChildObjectWithName("ContinueButton").GetComponent<Button>();
                _continueButtonText = _continueButton.gameObject.GetChildObjectWithName("ContinueButtonText").GetComponent<TextMeshProUGUI>();
                
                var rightHudGroup = gameObject.GetChildObjectWithName("RightHudGroup").gameObject;
                _goldUI = rightHudGroup.GetChildObjectWithName("GoldGroup").GetChildObjectWithName("Gold").GetComponent<TextMeshProUGUI>();
                _ropeGroup = rightHudGroup.GetChildObjectWithName("RopeGroup");
                _ropeUI = _ropeGroup.GetChildObjectWithName("Ropes").GetComponent<TextMeshProUGUI>();
                _torchGroup = rightHudGroup.GetChildObjectWithName("TorchGroup");
                _torchUI = _torchGroup.GetChildObjectWithName("Torches").GetComponent<TextMeshProUGUI>();

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
                _dialogueBox.gameObject.Disable();
                _goldUI.gameObject.Enable();
                _ropeGroup.Disable();
                _torchGroup.Disable();
                _healthUI.enabled = true;
                _healthBar.enabled = true;
                _hotbar.enabled = true;

                _healthBar.Value = _healthBar.Max;
                _promptText.text = string.Empty;
            }
            catch
            {
                // ignored
            }
        }

        public void ShowFloatingText(Vector2 location, string text, Color? color = null) => ShowFloatingText((Vector3) location, text, color);
        public void ShowFloatingText(Vector3 location, string text, Color? color = null) 
        {
            var t = Instantiate(FloatingTextDamagePrefab, location, Quaternion.identity).GetComponent<TextMeshPro>();
            t.text = text;
            t.color = color ?? Color.black;
        }

        public void ShowDialogue(string text, string name = "") 
        {
            _dialogueName.text = name;
            _dialogueText.text = text;

            _promptText.enabled = false;
            _dialogueBox.gameObject.Enable();
        }

        public void HideDialogue()
        {
            _dialogueBox.gameObject.Disable();
            if (_promptText.enabled == false && !_promptText.text.IsNullOrEmpty())
                _promptText.enabled = true;
        }

        public void ShowPrompt(string text) 
        {
            _promptText.enabled = true;
            _promptText.text = text;
        }

        public void HidePrompt()
        {
            _promptText.enabled = false;
            _promptText.text = string.Empty;
        }

        public void UpdateUi(float gold, float ropeQuantity, float torchQuantity, float health, float maxHealth)
        {
            try
            {
                _goldUI.text = gold.ToString();
                _healthUI.text = $"{(int) health} / {(int) maxHealth}";
                _healthBar.Value = health;

                if (ropeQuantity > 0)
                {
                    _ropeGroup.Enable();
                    _ropeUI.text = ropeQuantity.ToString();
                }
                else
                {
                    _ropeGroup.Disable();
                }

                if (torchQuantity > 0)
                {
                    _torchGroup.Enable();
                    _torchUI.text = Mathf.Floor(torchQuantity).ToString();
                }
                else
                {
                    _torchGroup.Disable();
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

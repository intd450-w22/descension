using Managers;
using UI.MenuUI;
using Util.Helpers;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;

namespace UI.Controllers
{
    public class HUDController : MonoBehaviour
    {
        [Header("UI Prefabs")]
        public GameObject FloatingTextDamagePrefab;
        public GameObject FloatingTextDialoguePrefab;

        private Text _notificationText;
        private Image _dialogueBox;
        private Text _dialogueName;
        private Text _dialogueText;
        private Button _continueButton;
        private Text _continueButtonText;
        private Text _scoreUI;
        private Text _bowUI;
        private Text _pickUI;
        private Text _torchUI;
        private Text _ropeUI;
        private Text _healthUI;
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
                _notificationText = gameObject.GetChildObjectWithName("NotificationText").GetComponent<Text>();
                _dialogueBox = gameObject.GetChildObjectWithName("DialogueBox").GetComponent<Image>();
                _dialogueName = _dialogueBox.gameObject.GetChildObjectWithName("DialogueBoxName").GetComponent<Text>();
                _dialogueText = _dialogueBox.gameObject.GetChildObjectWithName("DialogueBoxText").GetComponent<Text>();
                _continueButton = _dialogueBox.gameObject.GetChildObjectWithName("ContinueButton").GetComponent<Button>();
                _continueButtonText = _continueButton.gameObject.GetChildObjectWithName("Text").GetComponent<Text>();
                
                var rightHudGroup = gameObject.GetChildObjectWithName("RightHudGroup").gameObject;
                _scoreUI = rightHudGroup.GetChildObjectWithName("Score").GetComponent<Text>();
                _bowUI = null; // guiGroup.GetChildObjectWithName("BowDurability").GetComponent<Text>();
                _pickUI = null; // guiGroup.GetChildObjectWithName("PickDurability").GetComponent<Text>();
                _torchUI = rightHudGroup.GetChildObjectWithName("TorchDurability").GetComponent<Text>();
                _ropeUI = rightHudGroup.GetChildObjectWithName("RopeDurability").GetComponent<Text>();

                var leftHudGroup = gameObject.GetChildObjectWithName("LeftHudGroup");
                _healthUI = leftHudGroup.GetChildObjectWithName("Health").GetComponent<Text>();
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
                _notificationText.enabled = false;
                _dialogueBox.enabled = false;
                _dialogueName.enabled = false;
                _dialogueText.enabled = false;
                _continueButton.enabled = false;
                _continueButtonText.enabled = false;
                _scoreUI.enabled = true;
                _bowUI.enabled = false;
                _pickUI.enabled = false;
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
            _notificationText.enabled = false;
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
            _notificationText.enabled = false;
            _dialogueBox.enabled = true;
            _dialogueName.enabled = true;
            _dialogueText.enabled = true;
            _continueButton.enabled = true;
            _continueButtonText.enabled = true;

            _dialogueName.text = name;
            _dialogueText.text = text;
        }

        public void ShowNotification(string text) {
            _notificationText.enabled = true;
            _notificationText.text = text;
        }

        public void UpdateUi(float score, float pickQuantity, float arrowsQuantity, float ropeQuantity, float torchQuantity, float health)
        {
            try
            {
                _scoreUI.text = "Gold/Score: " + score.ToString();
                _healthUI.text = "Health: " + health.ToString();
                _healthBar.Value = health;

                if(_pickUI != null)
                    if (pickQuantity > 0)
                    {
                        _pickUI.enabled = true;
                        _pickUI.text = "Pick " + pickQuantity.ToString();
                    }
                    else
                    {
                        _pickUI.enabled = false;
                    }

                if (_bowUI != null)
                    if (arrowsQuantity > 0)
                    {
                        _bowUI.enabled = true;
                        _bowUI.text = "Arrows " + arrowsQuantity.ToString();
                    }
                    else
                    {
                        _bowUI.enabled = false;
                    }

                if (ropeQuantity > 0)
                {
                    _ropeUI.enabled = true;
                    _ropeUI.text = "Rope " + ropeQuantity.ToString();
                }
                else
                {
                    _ropeUI.enabled = false;
                }

                if (torchQuantity > 0)
                {
                    _torchUI.enabled = true;
                    _torchUI.text = "Torch " + Mathf.Floor(torchQuantity).ToString();
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

using System;
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

        [Header("Configuration")] 
        private float _codexNotificationTime = 4f;

        private TextMeshProUGUI _promptText;
        private Image _dialogueBox;
        private TextMeshProUGUI _dialogueName;
        private TextMeshProUGUI _dialogueText;
        private Button _continueButton;
        private TextMeshProUGUI _continueButtonText;

        private GameObject _healthGroup;
        private TextMeshProUGUI _healthText;
        private ProgressBar _healthBar;
        private TextMeshProUGUI _goldText;
        private GameObject _ropeGroup;
        private TextMeshProUGUI _ropeText;
        private GameObject _torchGroup;
        private TextMeshProUGUI _torchText;
        private GameObject _codexNotification;
        private TextMeshProUGUI _timerText;

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
                _promptText = gameObject.GetChildObject("PromptText").GetComponent<TextMeshProUGUI>();
                _dialogueBox = gameObject.GetChildObject("DialogueBox").GetComponent<Image>();
                _dialogueName = _dialogueBox.gameObject.GetChildObject("DialogueBoxName").GetComponent<TextMeshProUGUI>();
                _dialogueText = _dialogueBox.gameObject.GetChildObject("DialogueBoxText").GetComponent<TextMeshProUGUI>();
                _continueButton = _dialogueBox.gameObject.GetChildObject("ContinueButton").GetComponent<Button>();
                _continueButtonText = _continueButton.gameObject.GetChildObject("ContinueButtonText").GetComponent<TextMeshProUGUI>();
                
                var rightHudGroup = gameObject.GetChildObject("RightHudGroup").gameObject;
                _goldText = rightHudGroup.GetChildObject("GoldGroup").GetChildObject("Gold").GetComponent<TextMeshProUGUI>();
                _ropeGroup = rightHudGroup.GetChildObject("RopeGroup");
                _ropeText = _ropeGroup.GetChildObject("Ropes").GetComponent<TextMeshProUGUI>();
                _torchGroup = rightHudGroup.GetChildObject("TorchGroup");
                _torchText = _torchGroup.GetChildObject("Torches").GetComponent<TextMeshProUGUI>();

                var leftHudGroup = gameObject.GetChildObject("LeftHudGroup");
                _healthGroup = leftHudGroup.GetChildObject("HealthGroup");
                _healthText = _healthGroup.GetChildObject("Health").GetComponent<TextMeshProUGUI>();
                _healthBar = _healthGroup.GetChildObject("HealthBar").GetComponent<ProgressBar>();

                _codexNotification = gameObject.GetChildObject("CodexNotification");
                _timerText = gameObject.GetChildObject("TimerText").GetComponent<TextMeshProUGUI>();

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
                _codexNotification.Disable();
                _promptText.enabled = false;
                _dialogueBox.gameObject.Disable();
                _goldText.gameObject.Enable();
                _ropeGroup.Disable();
                _torchGroup.Disable();
                _healthText.enabled = true;
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
            location.z = 3f;
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

        public void ShowPrompt(string text, float time = 0) 
        {
            _promptText.enabled = true;
            _promptText.text = text;
        }

        public void HidePrompt()
        {
            _promptText.enabled = false;
            _promptText.text = string.Empty;
        }

        public void ShowCodexNotification() 
        {
            Invoke(nameof(HideCodexNotification), _codexNotificationTime);
            _codexNotification.Enable();
        }

        private void HideCodexNotification() => _codexNotification.Disable();

        public void UpdateUi(float gold, float ropeQuantity, float torchQuantity, float health, float maxHealth, float timer)
        {
            try
            {
                _goldText.text = gold.ToString();
                _healthText.text = $"{(int) health}/{(int) maxHealth}";
                _healthBar.Value = health;

                if (ropeQuantity > 0)
                {
                    _ropeGroup.Enable();
                    _ropeText.text = ropeQuantity.ToString();
                }
                else
                {
                    _ropeGroup.Disable();
                }

                if (torchQuantity > 0)
                {
                    _torchGroup.Enable();
                    _torchText.text = Mathf.Floor(torchQuantity).ToString();
                }
                else
                {
                    _torchGroup.Disable();
                }

                if (timer > 0)
                {
                    _timerText.enabled = true;
                    _timerText.text = string.Format("{0}:{1:00}", Mathf.Floor(Mathf.Floor(timer) / 60),
                        Mathf.Floor(timer) % 60);
                }
                else
                {
                    _timerText.enabled = false;
                }
            }
            catch (NullReferenceException e)
            {
                GameDebug.LogWarning(e.Message);
                UIManager.ReinitHudController();
            }
            catch (MissingReferenceException e)
            {
                GameDebug.LogWarning(e.Message);
                UIManager.ReinitHudController();
            }
            
        }
    }
}

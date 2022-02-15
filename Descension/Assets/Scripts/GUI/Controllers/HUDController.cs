using Assets.Scripts.Util.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Controllers
{
    public class HUDController : MonoBehaviour
    {
        [Header("UI Prefabs")]
        public GameObject FloatingTextDamagePrefab;
        public GameObject FloatingTextDialoguePrefab;

        private Image _dialogueBox;
        private Text _dialogueText;
        private Text _scoreUI;
        private Text _bowUI;
        private Text _pickUI;
        private Text _torchUI;
        private Text _ropeUI;

        void Awake()
        {
            SetReferences();
            Init();
        }

        private void SetReferences()
        {
            _dialogueBox = gameObject.GetChildObjectWithName("DialogueBox").GetComponent<Image>();
            _dialogueText = _dialogueBox.gameObject.GetChildObjectWithName("DialogueBoxText").GetComponent<Text>();
            var guiGroup = gameObject.GetChildObjectWithName("GuiGroup").gameObject;
            _scoreUI = guiGroup.GetChildObjectWithName("Score").GetComponent<Text>();
            _bowUI = guiGroup.GetChildObjectWithName("BowDurability").GetComponent<Text>();
            _pickUI = guiGroup.GetChildObjectWithName("PickDurability").GetComponent<Text>();
            _torchUI = guiGroup.GetChildObjectWithName("TorchDurability").GetComponent<Text>();
            _ropeUI = guiGroup.GetChildObjectWithName("RopeDurability").GetComponent<Text>();
        }

        private void Init()
        {
            _dialogueBox.enabled = false;
            _dialogueText.enabled = false;
            _scoreUI.enabled = true;
            _bowUI.enabled = false;
            _pickUI.enabled = false;
            _torchUI.enabled = false;
            _ropeUI.enabled = false;
        }

        public void ShowFloatingText(Vector2 location, string text, Color? color = null) => ShowFloatingText((Vector3) location, text, color);

        public void ShowFloatingText(Vector3 location, string text, Color? color = null) {
            var t = Instantiate(FloatingTextDamagePrefab, location, Quaternion.identity).GetComponent<TextMesh>();
            t.text = text;
            t.color = color ?? Color.black;
        }

        public void ShowText(string text) {
            _dialogueBox.enabled = true;
            _dialogueText.enabled = true;
            _dialogueText.text = text;
        }

        public void UpdateUi(float score, float pickQuantity, float arrowsQuantity, float ropeQuantity, float torchQuantity)
        {
            _scoreUI.text = "Gold/Score: " + score.ToString();

            if (pickQuantity > 0)
            {
                _pickUI.enabled = true;
                _pickUI.text = "Pick " + pickQuantity.ToString();
            }
            else
            {
                _pickUI.enabled = false;
            }

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
    }
}

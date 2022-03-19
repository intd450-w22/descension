using Managers;
using UI.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Environment
{
    public class InspectableObject : MonoBehaviour
    {
        public string inspectText;
        public Image dialogueBox;
        public Text dialogueText;

        public string name;
        public string[] linesOfDialogue;

        private bool playerInRange = false;

        private HUDController _hudController;
        private SoundManager _soundManager;
        private DialogueManager _dialogueManager;

        void Awake()
        {
            _hudController = UIManager.Instance.GetHudController();
            _soundManager = FindObjectOfType<SoundManager>();
            _dialogueManager = FindObjectOfType<DialogueManager>();
        }

        void Update() {
            if (playerInRange && Input.GetKeyDown(KeyCode.F)) {
                _soundManager.Inspection();
                // UIManager.Instance.GetHudController().ShowText(inspectText);
                _dialogueManager.StartDialogue(linesOfDialogue);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            playerInRange = true;
            UIManager.Instance.GetHudController().ShowText("Press F to interact");
        }

        private void OnTriggerExit2D(Collider2D other) {
            playerInRange = false;
            UIManager.Instance.GetHudController().HideDialogue();
        }
    }
}

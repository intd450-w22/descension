using Managers;
using UI.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Environment
{
    public class InspectableObject : MonoBehaviour
    {
        public string name;
        public string[] linesOfDialogue;

        private bool _playerInRange = false;
        private SoundManager _soundManager;
        private DialogueManager _dialogueManager;

        void Awake()
        {
            _soundManager = FindObjectOfType<SoundManager>();
            _dialogueManager = FindObjectOfType<DialogueManager>();
        }

        void Update() {
            if (_playerInRange && Input.GetKeyDown(KeyCode.F)) {
                _dialogueManager.StartDialogue(name, linesOfDialogue);
                _soundManager.Inspection();
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            _playerInRange = true;
            _dialogueManager.ShowNotification("Press F to interact");
        }

        private void OnTriggerExit2D(Collider2D other) {
            _playerInRange = false;
            _dialogueManager.ClearLines();
            _dialogueManager.HideDialogue();
        }
    }
}

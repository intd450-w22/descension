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
        
        void Update() {
            if (_playerInRange && Input.GetKeyDown(KeyCode.F)) {
                DialogueManager.StartDialogue(name, linesOfDialogue);
                SoundManager.Inspection();
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player"))
            {
                _playerInRange = true;
                DialogueManager.ShowNotification("Press F to interact");
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player"))
            {
                _playerInRange = false;
                DialogueManager.ClearLines();
                DialogueManager.HideDialogue();
            }
        }
    }
}

using Managers;
using Rules;
using UnityEngine;
using Util.Enums;

namespace Environment
{
    public class InspectableObject : MonoBehaviour
    {
        public string name;
        public string[] linesOfDialogue;

        public FactKey Fact;

        private bool _playerInRange = false;
        
        void Update() {
            if (_playerInRange && Input.GetKeyDown(KeyCode.F)) {
                DialogueManager.StartDialogue(name, linesOfDialogue);
                SoundManager.Inspection();

                FactManager.SetFact(Fact, true);
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

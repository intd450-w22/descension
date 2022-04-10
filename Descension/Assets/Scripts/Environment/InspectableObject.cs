using Managers;
using UnityEngine;
using Util.Enums;

namespace Environment
{
    public class InspectableObject : MonoBehaviour
    {
        public string name;
        public string[] linesOfDialogue;
        public bool destroyAfterInteraction = true;
        public FactKey Fact;

        private bool _inspected;
        private bool _playerInRange;
        
        void Update() {
            if (_playerInRange && Input.GetKeyDown(KeyCode.F))
            {   
                SoundManager.Inspection();
                FactManager.SetFact(Fact, true);
                
                DialogueManager.StartDialogue(name, linesOfDialogue, () =>
                {
                    // if (destroyAfterInteraction) Destroy(transform.parent.gameObject);
                });
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

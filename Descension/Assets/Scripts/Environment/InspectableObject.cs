using Actor.Interface;
using Managers;
using UnityEngine;
using Util.Enums;

namespace Environment
{
    public class InspectableObject : UniqueMonoBehaviour
    {
        public new string name;
        public string[] linesOfDialogue;
        
        public FactKey Fact;

        private bool _inspected;
        private bool _playerInRange;

        void Awake()
        {
            if (IsUniqueDestroyed()) Destroy(transform.parent.gameObject);
        }
        
        void Update() {
            if (GameManager.IsFrozen) return;

            // TODO maybe change input to input system
            if (_playerInRange && !_inspected && Input.GetKeyDown(KeyCode.F))
            {
                _inspected = true;
                
                SoundManager.Inspection();
                FactManager.SetFact(Fact, true);
                DestroyUniquePermanent();
                DialogueManager.StartDialogue(name, linesOfDialogue, () => Destroy(transform.parent.gameObject));
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player"))
            {
                _playerInRange = true;
                DialogueManager.ShowPrompt("Press F to interact");
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player"))
            {
                _playerInRange = false;
                DialogueManager.HidePrompt();
            }
        }
    }
}

using Managers;
using UnityEngine;

namespace Environment
{
    public class BombScript : MonoBehaviour
    {
        public string name;
        public string[] linesOfDialogue;
        public KeyCode interactionKey = KeyCode.F;
        private bool _inRange;
        private bool _hasExplosives;
        private bool _hasTrigger;
        private bool _hasTimer;

        private static BombScript _instance;
        public static BombScript Instance => _instance ? _instance : _instance = FindObjectOfType<BombScript>();

        public void AddExplosives() => _hasExplosives = true;
        public void AddTrigger() => _hasTrigger = true;
        public void AddTimer() => _hasTimer = true;
        
        void Update() {
            if (_inRange && Input.GetKeyDown(interactionKey)) {
                DialogueManager.StartDialogue(name, linesOfDialogue);
                SoundManager.Inspection();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                DialogueManager.ShowNotification("Press F to interact");
                _inRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                DialogueManager.ClearLines();
                DialogueManager.HideDialogue();
                _inRange = false;
            }
        }
    }
}

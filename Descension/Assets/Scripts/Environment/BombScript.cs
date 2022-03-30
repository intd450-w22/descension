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
        private bool hasExplosives;
        private bool hasTrigger;

        private static BombScript _instance;
        public static BombScript Instance = _instance ? _instance : _instance = FindObjectOfType<BombScript>();

        public void AddExplosives() => hasExplosives = true;
        public void AddTrigger() => hasTrigger = true;
        
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

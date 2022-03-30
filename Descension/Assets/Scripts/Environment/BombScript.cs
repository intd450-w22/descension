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

        void Update() {
            if (_inRange && Input.GetKeyDown(interactionKey)) {
                DialogueManager.StartDialogue(name, linesOfDialogue);
                SoundManager.Inspection();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            DialogueManager.ShowNotification("Press F to interact");   
            if (other.gameObject.CompareTag("Player")) _inRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            DialogueManager.ClearLines();
            DialogueManager.HideDialogue();
            if (other.gameObject.CompareTag("Player")) _inRange = false;
        }
    }
}

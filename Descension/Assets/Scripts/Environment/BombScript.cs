using Actor.Player;
using Managers;
using UI.Controllers;
using Util;
using Util.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Environment
{
    public class BombScript : MonoBehaviour
    {
        public string name;
        public string[] linesOfDialogue;
        public KeyCode interactionKey = KeyCode.F;
        private bool _inRange;

        void Update() {
            Debug.Log(_inRange);
            if (_inRange && Input.GetKeyDown(interactionKey)) {
                DialogueManager.StartDialogue(name, linesOfDialogue);
                SoundManager.Inspection();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("enter");
            DialogueManager.ShowNotification("Press F to interact");   
            if (other.gameObject.CompareTag("Player")) _inRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("exit");
            DialogueManager.ClearLines();
            DialogueManager.HideDialogue();
            if (other.gameObject.CompareTag("Player")) _inRange = false;
        }
    }
}

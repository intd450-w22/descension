using Managers;
using UI.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Environment
{
    public class Message : MonoBehaviour
    {
        public new string name;
        public string[] linesOfDialogue;
        public Image dialogueBox;
        public Text dialogueText;

        public bool triggered = false;

        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.GetHudController();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!triggered) {
                DialogueManager.StartDialogue(name, linesOfDialogue);
                SoundManager.Inspection();

                triggered = true;
            }
        }
    }
}

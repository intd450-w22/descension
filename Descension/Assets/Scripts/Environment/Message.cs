using UI.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Environment
{
    public class Message : MonoBehaviour
    {
        public string textToShow;
        public Image dialogueBox;
        public Text dialogueText;

        public bool triggered = false;

        private HUDController _hudController;

        void Awake()
        {
            _hudController = FindObjectOfType<HUDController>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!triggered) {
                _hudController.ShowText(textToShow);
                triggered = true;
            }
        }
    }
}

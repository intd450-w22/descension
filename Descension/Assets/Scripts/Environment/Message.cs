using Managers;
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
            _hudController = UIManager.Instance.GetHudController();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!triggered) {
                UIManager.Instance.GetHudController().ShowText(textToShow);
                triggered = true;
            }
        }
    }
}

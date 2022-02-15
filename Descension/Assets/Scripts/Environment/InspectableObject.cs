using Managers;
using UI.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Environment
{
    public class InspectableObject : MonoBehaviour
    {
        public string inspectText;
        public Image dialogueBox;
        public Text dialogueText;

        private bool playerInRange = false;

        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.Instance.GetHudController();
        }

        void Update() {
            if (playerInRange && Input.GetKeyDown(KeyCode.F)) {
                _hudController.ShowText(inspectText);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            playerInRange = true;
            _hudController.ShowText("Press F to interact");
        }

        private void OnTriggerExit2D(Collider2D other) {
            playerInRange = false;
        }
    }
}

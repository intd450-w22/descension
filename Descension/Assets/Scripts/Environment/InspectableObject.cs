using Assets.Scripts.GUI.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Environment
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
            _hudController = FindObjectOfType<HUDController>();
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

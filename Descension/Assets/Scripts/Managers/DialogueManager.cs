using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Managers;
using UI.Controllers;
using UnityEngine.UI;

namespace UI.Controllers
{
    public class DialogueManager : MonoBehaviour
    {
        private float index = 0;
        private Queue<string> linesOfDialogue;

        private HUDController _hudController;

        // Start is called before the first frame update
        void Start()
        {
            linesOfDialogue = new Queue<string>();
            _hudController = UIManager.Instance.GetHudController();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void StartDialogue(string[] lines) {
            Debug.Log("starting conversation");
            linesOfDialogue.Clear();

            foreach (string line in lines) {
                linesOfDialogue.Enqueue(line);
            }

            DisplayNextLine();
        }

        public void DisplayNextLine() {
            Debug.Log("Continuing dialogue");

            if (linesOfDialogue.Count == 0) {
                _hudController.HideDialogue();
            } else {
                _hudController.ShowText(linesOfDialogue.Dequeue());
            }
        }

        // private void OnTriggerEnter2D(Collider2D other) {
        //     playerInRange = true;
        //     UIManager.Instance.GetHudController().ShowText("Press F to interact");
        // }

        // private void OnTriggerExit2D(Collider2D other) {
        //     playerInRange = false;
        //     UIManager.Instance.GetHudController().HideDialogue();
        // }
    }
}


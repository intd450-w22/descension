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
        private string _name  = "";
        private Queue<string> _linesOfDialogue = new Queue<string>();
        private HUDController _hudController;

        void Awake() {
            _hudController = UIManager.Instance.GetHudController();
        }

        void Update() {
            if (Input.GetKeyDown("space")) {
                DisplayNextLine();
            }
        }

        public void StartDialogue(string objectName, string[] lines) {
            Time.timeScale = 0; // pause the game

            _name = objectName;
            _linesOfDialogue.Clear();

            foreach (string line in lines) {
                _linesOfDialogue.Enqueue(line);
            }

            DisplayNextLine();
        }

        public void StartDialogue(string objectName, List<string> lines) {
            Time.timeScale = 0; // pause the game

            _name = objectName;
            _linesOfDialogue.Clear();

            foreach (string line in lines) {
                _linesOfDialogue.Enqueue(line);
            }

            DisplayNextLine();
        }

        public void DisplayNextLine() {
            if (_linesOfDialogue.Count == 0) {
                Time.timeScale = 1; // resume the game
                _hudController.HideDialogue();
            } else {
                _hudController.ShowText(_linesOfDialogue.Dequeue(), _name);
            }
        }

        public void ClearLines() {
            _linesOfDialogue.Clear();
        }

        public void HideDialogue() {
            _hudController.HideDialogue();
        }

        public void ShowNotification(string text) {
            _hudController.ShowNotification(text);
        }
    }
}


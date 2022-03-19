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
        private string _name;
        
        private Queue<string> _linesOfDialogue;
        private HUDController _hudController;

        // Start is called before the first frame update
        void Start() {
            _name = "";
            _linesOfDialogue = new Queue<string>();
            _hudController = UIManager.Instance.GetHudController();
        }

        public void StartDialogue(string objectName, string[] lines) {
            Time.timeScale = 0; // pauses the game

            _name = objectName;
            _linesOfDialogue.Clear();

            foreach (string line in lines) {
                _linesOfDialogue.Enqueue(line);
            }

            DisplayNextLine();
        }

        public void DisplayNextLine() {
            if (_linesOfDialogue.Count == 0) {
                Time.timeScale = 1; // resumes the game

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


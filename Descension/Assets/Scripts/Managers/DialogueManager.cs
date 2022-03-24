using System.Collections.Generic;
using UI.Controllers;
using UnityEngine;

namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        private string _name  = "";
        private Queue<string> _linesOfDialogue = new Queue<string>();
        private static DialogueManager _instance;
        private static DialogueManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DialogueManager>();
                }
                return _instance;
            }
        }

        void Awake() {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        void Update() {
            if (Input.GetKeyDown("space")) {
                DisplayNextLine();
            }
        }

        public static void StartDialogue(string objectName, string[] lines) => Instance._StartDialogue(objectName, lines);
        private void _StartDialogue(string objectName, string[] lines) {
            Time.timeScale = 0; // pause the game

            _name = objectName;
            _linesOfDialogue.Clear();

            foreach (string line in lines) {
                _linesOfDialogue.Enqueue(line);
            }

            _DisplayNextLine();
        }
        
        public static void StartDialogue(string objectName, List<string> lines) => Instance._StartDialogue(objectName, lines);
        private void _StartDialogue(string objectName, List<string> lines) {
            Time.timeScale = 0; // pause the game

            _name = objectName;
            _linesOfDialogue.Clear();

            foreach (string line in lines) {
                _linesOfDialogue.Enqueue(line);
            }
            
            _DisplayNextLine();
        }
        
        public static void DisplayNextLine() => Instance._DisplayNextLine();
        private void _DisplayNextLine() {
            if (_linesOfDialogue.Count == 0) {
                Time.timeScale = 1; // resume the game
                UIManager.GetHudController().HideDialogue();
            } else {
                UIManager.GetHudController().ShowText(_linesOfDialogue.Dequeue(), _name);
            }
        }
        
        public static void ClearLines() => Instance._ClearLines();
        private void _ClearLines() {
            _linesOfDialogue.Clear();
        }

        public static void HideDialogue() => Instance._HideDialogue();
        private void _HideDialogue() {
            UIManager.GetHudController().HideDialogue();
        }

        public static void ShowNotification(string text) => Instance._ShowNotification(text);
        private void _ShowNotification(string text) {
            UIManager.GetHudController().ShowNotification(text);
        }
    }
}


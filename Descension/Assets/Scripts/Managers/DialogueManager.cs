using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        private string _name = "";
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

        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }

        void Update()
        {
            if (GameManager.IsPaused) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space");
                DisplayNextLine();
            }
        }

        public static void StartDialogue(string objectName, IEnumerable<string> lines) => Instance._StartDialogue(objectName, lines);
        private void _StartDialogue(string objectName, IEnumerable<string> lines)
        {
            GameManager.Freeze();

            _name = objectName;
            _linesOfDialogue.Clear();

            foreach (var line in lines)
            {
                _linesOfDialogue.Enqueue(line);
            }

            _DisplayNextLine();
        }

        public static void DisplayNextLine() => Instance._DisplayNextLine();
        private void _DisplayNextLine()
        {
            if (_linesOfDialogue.Count == 0)
            {
                GameManager.UnFreeze();
                UIManager.GetHudController().HideDialogue();
            }
            else
            {
                UIManager.GetHudController().ShowText(_linesOfDialogue.Dequeue(), _name);
            }
        }

        public static void ClearLines() => Instance._ClearLines();
        private void _ClearLines()
        {
            _linesOfDialogue.Clear();
        }

        public static void HideDialogue() => Instance._HideDialogue();
        private void _HideDialogue()
        {
            UIManager.GetHudController().HideDialogue();
        }

        // TODO: Change the name of this, these aren't really notifications, but prompts
        public static void ShowNotification(string text) => Instance._ShowNotification(text);
        private void _ShowNotification(string text)
        {
            UIManager.GetHudController().ShowNotification(text);
        }
    }
}


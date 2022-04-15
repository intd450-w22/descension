using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Util.Enums;

namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        private string _name = "";
        private readonly Queue<string> _linesOfDialogue = new Queue<string>();
        private Action _onDialogueComplete;

        public static bool IsInDialogue => Instance._isInDialogue;
        private bool _isInDialogue = false;

        private static DialogueManager _instance;
        private static DialogueManager Instance => _instance ??= FindObjectOfType<DialogueManager>();

        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }

        void Update()
        {
            if (!IsInDialogue || GameManager.IsPaused) return;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                DisplayNextLine();
            }
        }

        public static void StartDialogue(string objectName, IEnumerable<string> lines, Action onComplete = null) => Instance._StartDialogue(objectName, lines, onComplete);
        private void _StartDialogue(string objectName, IEnumerable<string> lines, Action onComplete)
        {
            GameManager.Freeze();

            _name = objectName;
            _linesOfDialogue.Clear();
            _onDialogueComplete = onComplete;
            _isInDialogue = true;

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
                HideDialogue();
                _onDialogueComplete?.Invoke();
                _onDialogueComplete = null;
                _isInDialogue = false;
            }
            else
            {
                var dialogue = _linesOfDialogue.Dequeue();
                if (dialogue.StartsWith(">"))
                {
                    try
                    {
                        var key = (DialogueKey) Enum.Parse(typeof(DialogueKey), dialogue.Substring(1), true);

                        switch (key)
                        {
                            case DialogueKey.OpenShop:
                                ItemShop.OpenShop();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    catch { /* ignored */ }
                }
                else
                {
                    UIManager.GetHudController().ShowDialogue(dialogue, _name);
                }
            }
        }

        public static void ClearLines() => Instance._ClearLines();
        private void _ClearLines() => _linesOfDialogue.Clear();

        public static void HideDialogue() => UIManager.GetHudController().HideDialogue();
        public static void HidePrompt() => UIManager.GetHudController().HidePrompt();

        // TODO: Change the name of this, these aren't really notifications, but prompts
        public static void ShowPrompt(string text) => Instance._ShowPrompt(text);
        private void _ShowPrompt(string text)
        {
            UIManager.GetHudController().ShowPrompt(text);
        }
    }
}


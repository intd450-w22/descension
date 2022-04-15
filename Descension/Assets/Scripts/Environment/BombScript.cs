using Managers;
using UnityEngine;
using UI.Controllers;
using Util.Enums;
using System;

namespace Environment
{
    public class BombScript : MonoBehaviour
    {
        public string name;
        public string[] linesOfDialogue;
        public string[] blowUpDialogue;
        public string[] startTimerDialogue;
        public KeyCode interactionKey = KeyCode.F;
        public KeyCode startBombKey = KeyCode.E;
        private bool _inRange;
        private bool _hasExplosives;
        private bool _hasTrigger;
        private bool _hasTimer;
        private bool _hasAll;
        private bool _hasExplosivesAndTrigger;
        private bool _activatedBombWithoutTrigger;
        public FactKey Fact;
        private static BombScript _instance;
        public static BombScript Instance => _instance ? _instance : _instance = FindObjectOfType<BombScript>();
        private Action _endGame;


        public void AddExplosives()
        {
            _hasExplosives = true;
            _hasExplosivesAndTrigger = _hasTrigger && _hasExplosives;
            _hasAll = _hasTimer && _hasTrigger && _hasExplosives;
        }

        public void AddTrigger()
        {
            _hasTrigger = true;
            _hasExplosivesAndTrigger = _hasTrigger && _hasExplosives;
            _hasAll = _hasTimer && _hasTrigger && _hasExplosives;
        }

        public void AddTimer()
        {
            _hasTimer = true;
            _hasAll = _hasTimer && _hasTrigger && _hasExplosives;
        }

        void Update() {
            if (GameManager.IsFrozen) return;

            if (_inRange)
            {
                if (Input.GetKeyDown(interactionKey)) 
                {
                    if (_hasAll) DialogueManager.StartDialogue(name, startTimerDialogue);
                    else if (_hasExplosivesAndTrigger) DialogueManager.StartDialogue(name, blowUpDialogue);
                    else DialogueManager.StartDialogue(name, linesOfDialogue);
                    SoundManager.Inspection();
                }
                else if (Input.GetKeyDown(startBombKey)) 
                {
                    if (_hasAll)
                    {
                        DialogueManager.StartDialogue(name, new [] {"Timer Started, RUN!!"});
                        FactManager.SetFact(Fact, true);
                    } 
                    else if (_hasExplosivesAndTrigger)
                    {
                        _endGame += EndGame;
                        DialogueManager.StartDialogue(name, new [] { "A hero was lost at the heart of the Descent. Though forgotten, their sacrifice will always be remembered by those who will never have to suffer." }, _endGame);
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                DialogueManager.ShowPrompt("Press F to interact");
                _inRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                DialogueManager.HidePrompt();
                _inRange = false;
            }
        }
        private void EndGame()
        {
            UIManager.SwitchUi(UIType.End);
        }
    }
}

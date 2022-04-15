using Actor.Interface;
using Managers;
using UnityEngine;
using UI.Controllers;
using Util.Enums;
using System;
using System.Collections.Generic;
using Actor.Player;

namespace Environment
{
    public class BombScript : AInteractable
    {
        [Header("Configuration")]
        public KeyCode StartBombKey = KeyCode.T;
        [SerializeField] public FactKey Fact;
        [SerializeField] public float _timeToEscape;
        
        [Header("Dialogue")]
        public string Name = "Bomb";
        // public string[] LinesOfDialogue;
        // public string[] BlowUpDialogue;
        // public string[] StartTimerDialogue;

        private bool _hasExplosives;
        private bool _hasTrigger;
        private bool _hasTimer;
        private bool _hasExplosivesAndTrigger => _hasExplosives && _hasTrigger;
        private bool _hasAll => _hasExplosives && _hasTrigger && _hasTimer;

        private bool _activatedBomb = false;
        private Action _endGame;

        private static BombScript _instance;
        public static BombScript Instance => _instance ? _instance : _instance = FindObjectOfType<BombScript>();
        
        public void AddExplosives()=> _hasExplosives = true;
        public void AddTrigger() => _hasTrigger = true;
        public void AddTimer() => _hasTimer = true;

        void Update() {
            if (GameManager.IsFrozen || !_inRange) return;

            if (Input.GetKeyDown(StartBombKey)) 
            {
                if (_hasAll && !_activatedBomb)
                {
                    DialogueManager.StartDialogue(name, new [] {"Timer Started, RUN!!"}, StartBomb);
                } 
                else if (_hasExplosivesAndTrigger && !_activatedBomb)
                {
                    _endGame += EndGame;
                    _activatedBomb = true;
                    DialogueManager.StartDialogue(name, new [] { "A hero was lost at the heart of the Descent. Though forgotten, their sacrifice will always be remembered by those who will never have to suffer." }, _endGame);
                }
            }
        }

        public void StartBomb()
        {
            if (_activatedBomb) return;

            FactManager.SetFact(Fact, true);
            _activatedBomb = true;
            PlayerController.StartTimer(_timeToEscape);
        }

        private void EndGame()
        {
            UIManager.SwitchUi(UIType.End);
        }

        public void InteractBomb()
        {
            SoundManager.Inspection();

            if (_hasAll) 
                DialogueManager.StartDialogue(Name, new List<string>
                {
                    "Explosives, a timer, and the trigger, that's everything from the note."
                });
            else if (_hasExplosivesAndTrigger) 
                DialogueManager.StartDialogue(Name, new List<string>
                {
                    "Explosives and the trigger, I could probably blow this up... but there'd be no leaving."
                });
            else if (_hasExplosives && !_hasTrigger)
                DialogueManager.StartDialogue(Name, new List<string>
                {
                    "There must be some way to detonate these explosives..."
                });
            else if (!_hasExplosives && (_hasTrigger || _hasTimer))
                DialogueManager.StartDialogue(Name, new List<string>
                {
                    "Kind of useless without any explosives..."
                });
            else 
                DialogueManager.StartDialogue(Name, new List<string>
                {
                    "It's as big as a carriage. So many clockwork parts, waiting and still. There are parts missing. There's a note on the ground by a body...",
                    "\"It's like they know. They ran off with the explosive charges and the trigger for the device.\"",
                    "\"Without those, the device won't detonate. Jones dropped the timer so even if we did have other parts, there'd be no leaving.\""
                });
        }

        public override void Interact() => InteractBomb();
        public override Vector2 Location() => gameObject.transform.position;
        public override string GetPrompt()
        {
            if (_activatedBomb) return string.Empty;
            return _hasExplosivesAndTrigger ? "Press T to detonate   Press F to interact" : "Press F to interact";
        }
    }
}

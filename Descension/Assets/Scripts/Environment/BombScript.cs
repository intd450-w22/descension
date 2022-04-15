using Actor.Interface;
using Managers;
using UnityEngine;
using UI.Controllers;
using Util.Enums;
using System;
using Actor.Player;

namespace Environment
{
    public class BombScript : AInteractable
    {
        [Header("Configuration")]
        public KeyCode StartBombKey = KeyCode.T;
        
        [Header("Dialogue")]
        public string Name;
        public string[] LinesOfDialogue;
        public string[] BlowUpDialogue;
        public string[] StartTimerDialogue;

        private bool _hasExplosives;
        private bool _hasTrigger;
        private bool _hasTimer;
        private bool _hasExplosivesAndTrigger => _hasExplosives && _hasTrigger;
        private bool _hasAll => _hasExplosives && _hasTrigger && _hasTimer;

        private bool _activatedBomb = false;
        public FactKey Fact;
        private Action _endGame;
        public float timeToEscape;

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
                    DialogueManager.StartDialogue(name, new [] {"Timer Started, RUN!!"});
                    FactManager.SetFact(Fact, true);
                    _activatedBomb = true;
                    PlayerController.StartTimer(timeToEscape);
                } 
                else if (_hasExplosivesAndTrigger && !_activatedBomb)
                {
                    _endGame += EndGame;
                    _activatedBomb = true;
                    DialogueManager.StartDialogue(name, new [] { "A hero was lost at the heart of the Descent. Though forgotten, their sacrifice will always be remembered by those who will never have to suffer." }, _endGame);
                }
            }
        }

        private void EndGame()
        {
            UIManager.SwitchUi(UIType.End);
        }

        public void Bomb()
        {
            SoundManager.Inspection();

            if (_hasAll) 
                DialogueManager.StartDialogue(Name, StartTimerDialogue);
            else if (_hasExplosivesAndTrigger) 
                DialogueManager.StartDialogue(Name, BlowUpDialogue);
            else 
                DialogueManager.StartDialogue(Name, LinesOfDialogue);
        }

        public override void Interact() => Bomb();
        public override Vector2 Location() => gameObject.transform.position;
        public override string GetPrompt() => "Press T to detonate   Press F to interact";        
    }
}

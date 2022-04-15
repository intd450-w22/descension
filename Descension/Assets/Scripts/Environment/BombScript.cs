using Actor.Interface;
using Managers;
using UnityEngine;

namespace Environment
{
    public class BombScript : AInteractable
    {
        [Header("Configuration")]
        public KeyCode StartBombKey = KeyCode.E;
        
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
        
        private static BombScript _instance;
        public static BombScript Instance => _instance ? _instance : _instance = FindObjectOfType<BombScript>();
        
        public void AddExplosives()=> _hasExplosives = true;
        public void AddTrigger() => _hasTrigger = true;
        public void AddTimer() => _hasTimer = true;

        void Update() {
            if (GameManager.IsFrozen || !_inRange) return;

            if (Input.GetKeyDown(StartBombKey)) 
            {
                if (_hasAll)
                    DialogueManager.StartDialogue(Name, new [] {"Timer Started, RUN!!"});
                else if (_hasExplosivesAndTrigger)
                    DialogueManager.StartDialogue(Name, new [] {"BOOM!"});
            }
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
        public override string GetPrompt() => "Press F to interact";
    }
}

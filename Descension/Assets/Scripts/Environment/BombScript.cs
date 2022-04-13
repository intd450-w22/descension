using Managers;
using UnityEngine;

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
        private static BombScript _instance;
        public static BombScript Instance => _instance ? _instance : _instance = FindObjectOfType<BombScript>();
        
        
        
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
                    } 
                    else if (_hasExplosivesAndTrigger)
                    {
                        DialogueManager.StartDialogue(name, new [] {"BOOM!"});
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
    }
}

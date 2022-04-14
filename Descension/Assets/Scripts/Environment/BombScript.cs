using Managers;
using UnityEngine;
using UI.Controllers;
using Util.Enums;

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
            if (GameManager.IsFrozen) return;

            if (_inRange)
            {
                if (_activatedBombWithoutTrigger)
                {
                    UIManager.SwitchUi(UIType.End);
                }
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
                        DialogueManager.StartDialogue(name, new [] { "A hero was lost at the heart of the Descent. Though forgotten, their sacrifice will always be remembered by those who will never have to suffer." });
                        _activatedBombWithoutTrigger = true;
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

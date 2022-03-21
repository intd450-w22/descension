using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEngine;

namespace Items
{
    public class torchItem : MonoBehaviour
    {
        public float quantity = 20;
        
        private bool _isPickedUp = false;
        private DialogueManager _dialogueManager;

        private string[] _description = 
            new string[] {"Torch Collected. Use with caution.", "There are things down here that have more eyes than you.", 
            "Press Q to toggle it on and off."};

        void Awake()
        {
            _dialogueManager = FindObjectOfType<DialogueManager>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isPickedUp) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                _isPickedUp = true;
                FindObjectOfType<SoundManager>().ItemFound();
                FindObjectOfType<PlayerController>().AddTorch(quantity);
                _dialogueManager.StartDialogue("Torch", _description);
                Destroy(gameObject);
            }
        }
    }
}

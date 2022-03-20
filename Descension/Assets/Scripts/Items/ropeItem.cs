using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEngine;

namespace Items
{
    public class ropeItem : MonoBehaviour
    {
        public float quantity = 1;
        public Vector2[] potentialPositions;

        private bool _isPickedUp = false; 
        private DialogueManager _dialogueManager;

        private string[] _description = 
            new string[] {"Rope Collected.", "Necessary for going further into the mines, or other things too."};

        void Awake()
        {
            // set a random starting location
            gameObject.transform.position = potentialPositions[UnityEngine.Random.Range(0, potentialPositions.Length)];
            _dialogueManager = FindObjectOfType<DialogueManager>();
        }

        void OnCollisionEnter2D(Collision2D collision) 
        {
            if (_isPickedUp) return;

            FindObjectOfType<SoundManager>().ItemFound();
            if (collision.gameObject.CompareTag("Player"))
            {
                _isPickedUp = true;
                FindObjectOfType<PlayerController>().AddRope(quantity);
                _dialogueManager.StartDialogue("Rope", _description);
                Destroy(gameObject);
            }
        }

    }
}

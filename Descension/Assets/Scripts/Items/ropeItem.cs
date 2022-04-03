using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;

namespace Items
{
    public class ropeItem : MonoBehaviour
    {
        public int quantity = 1;
        public Vector2[] potentialPositions;

        private bool _isPickedUp = false;

        private string[] _description = 
            new string[] {"Rope Collected.", "Be sure to hold on with both hands while descending. When ascending...well...God help you."};

        void Awake()
        {
            // set a random starting location
            gameObject.transform.position = potentialPositions[UnityEngine.Random.Range(0, potentialPositions.Length)];
        }

        void OnCollisionEnter2D(Collision2D collision) 
        {
            if (_isPickedUp) return;

            SoundManager.ItemFound();
            if (collision.gameObject.CompareTag("Player"))
            {
                _isPickedUp = true;
                PlayerController.AddRope(quantity);
                DialogueManager.StartDialogue("Rope", _description);
                FactManager.SetFact(FactKey.HasSeenRope, true);
                Destroy(gameObject);
            }
        }

    }
}

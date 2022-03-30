using Actor.Player;
using Managers;
using Rules;
using UI.Controllers;
using UnityEngine;
using Util.Enums;

namespace Items
{
    public class torchItem : MonoBehaviour
    {
        public float quantity = 20;
        
        private bool _isPickedUp = false;

        private string[] _description = 
            new string[] {"Torch Collected. Use with caution.", "There are things down here that have more eyes than you.", 
            "Press Q to toggle it on and off."};

        private static bool _hasSeenTorch;

        void Awake() { }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isPickedUp) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                _isPickedUp = true;
                SoundManager.ItemFound();
                GameManager.PlayerController.AddTorch(quantity);
                if(!_hasSeenTorch)
                    DialogueManager.StartDialogue("Torch", _description);
                FactManager.SetFact(FactKey.HasSeenTorch, true);
                _hasSeenTorch = true;
                Destroy(gameObject);
            }
        }
    }
}

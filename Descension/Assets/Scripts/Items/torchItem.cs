using System;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;

namespace Items
{
    public class torchItem : MonoBehaviour
    {
        public int quantity = 20;
        
        private bool _isPickedUp = false;

        private string[] _description = 
            new string[] {"Torch Collected. Use with caution.", "There are things down here that have more eyes than you.", 
            "Press Q to toggle it on and off."};

        private static bool _hasSeenTorch;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isPickedUp) return;

            if (other.gameObject.CompareTag("Player"))
            {
                _isPickedUp = true;
                SoundManager.ItemFound();
                PlayerController.AddTorch(quantity);
                if(!_hasSeenTorch)
                    DialogueManager.StartDialogue("Torch", _description);
                FactManager.SetFact(FactKey.HasSeenTorch, true);
                _hasSeenTorch = true;
                Destroy(gameObject);
            }
        }
    }
}

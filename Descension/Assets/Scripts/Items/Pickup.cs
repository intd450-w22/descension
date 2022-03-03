using System;
using Environment;
using Managers;
using UnityEngine;

namespace Items
{
    public class Pickup : MonoBehaviour
    {
        public EquippableItem item;
        public int quantity = 1;
        public String pickupMessage;
        private bool _isPickedUp;
        
        private void OnValidate()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = item.inventorySprite;
        }


        void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isPickedUp) return;
    
            if (collision.gameObject.CompareTag("Player"))
            {
                _isPickedUp = true;
                SoundManager.Instance.ItemFound();
                InventoryManager.Instance.PickupItem(item, quantity);
                UIManager.Instance.GetHudController().ShowText(pickupMessage);
                Destroy(gameObject);
            }
        }
    }
}

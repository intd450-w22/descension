using System;
using Environment;
using Managers;
using UnityEngine;

namespace Items.Pickups
{
    public class Pickup : MonoBehaviour
    {
        public EquippableItem item;
        public int quantity = 1;
        public String pickupMessage;
        private bool _inRange;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) && _inRange)
            {
                SoundManager.Instance.ItemFound();
                InventoryManager.Instance.PickupItem(item, quantity);
                UIManager.Instance.GetHudController().ShowText(pickupMessage);
                Destroy(gameObject);
            }

        }

        private void OnValidate()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = item.inventorySprite;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")) _inRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")) _inRange = false;
        }
    }
}

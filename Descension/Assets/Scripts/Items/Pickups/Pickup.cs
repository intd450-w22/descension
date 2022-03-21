using System;
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
            if (Input.GetKeyDown(KeyCode.E) && _inRange)
            {
                if (!InventoryManager.Instance.PickupItem(item, ref quantity))
                {
                    SoundManager.Instance.Error(); //TODO fail to pick up sound
                    UIManager.Instance.GetHudController().ShowText("Inventory full");
                    return;
                }
                SoundManager.Instance.ItemFound();
                UIManager.Instance.GetHudController().ShowText(pickupMessage);

                if (quantity == 0) Destroy(gameObject);
            }

        }

        private void OnValidate()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = item.inventorySprite;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            UIManager.Instance.GetHudController().ShowText("Press E to collect " + item.GetName());
            if (other.gameObject.CompareTag("Player")) _inRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            UIManager.Instance.GetHudController().HideDialogue();
            if (other.gameObject.CompareTag("Player")) _inRange = false;
        }
    }
}

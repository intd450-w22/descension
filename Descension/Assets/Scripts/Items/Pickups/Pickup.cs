using Managers;
using UnityEngine;

namespace Items.Pickups
{
    public class Pickup : MonoBehaviour
    {
        public EquippableItem item;
        public int quantity = 1;
        public string pickupMessage;
        private bool _inRange;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && _inRange)
            {
                if (!InventoryManager.PickupItem(item, ref quantity))
                {
                    SoundManager.Error(); //TODO fail to pick up sound
                    UIManager.GetHudController().ShowText("Inventory full");
                    return;
                }
                SoundManager.ItemFound();
                UIManager.GetHudController().ShowText(pickupMessage);

                if (quantity == 0) Destroy(gameObject);
            }
        }

        private void OnValidate()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = item.inventorySprite;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.CompareTag("Player"))
            {
                UIManager.GetHudController().ShowText("Press E to collect " + item.GetName());
                _inRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                UIManager.GetHudController().HideDialogue();
                _inRange = false;
            }
        }
    }
}

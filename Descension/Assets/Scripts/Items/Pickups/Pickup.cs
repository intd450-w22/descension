using Managers;
using UnityEngine;
using Util.Helpers;

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
                
                if (quantity == 0) Destroy(gameObject);
                
                // only show pickup dialogue once
                if (!FactManager.IsFactTrue(item.Fact))
                {
                    UIManager.GetHudController().ShowText(pickupMessage);
                    FactManager.SetFact(item.Fact, true);
                }
            }
        }

        private void OnValidate()
        {
            gameObject.GetChildObjectWithName("ItemSprite").GetComponent<SpriteRenderer>().sprite = item.inventorySprite;
            // gameObject.GetComponentInChildren<SpriteRenderer>().sprite = item.inventorySprite;
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

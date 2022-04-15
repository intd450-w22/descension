using Actor.Interface;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.Helpers;

namespace Items.Pickups
{
    public class Pickup : UniqueMonoBehaviour
    {
        public EquippableItem item;
        public int quantity = 1;
        public string[] pickupMessage;
        public bool autoPickup;
        private bool _inRange;

        protected void Awake()
        {
            if (IsUniqueDestroyed()) Destroy(gameObject);
        }
        
        protected new void OnEnable() 
        {
            if (GetUniqueId() == 0) GetNewUniqueId();
            
            base.OnEnable();
        }
        
        private void Update()
        {
            if (_inRange && (autoPickup || Input.GetKeyDown(KeyCode.E)))
            {
                if (!InventoryManager.PickupItem(item, ref quantity))
                {
                    SoundManager.Error(); //TODO fail to pick up sound
                    UIManager.GetHudController().ShowDialogue("Inventory full");
                    return;
                }
                SoundManager.ItemFound();

                if (quantity == 0)
                {
                    DestroyUnique();
                    Destroy(gameObject);
                }
                
                // only show pickup dialogue once
                if (!FactManager.IsFactTrue(item.Fact))
                {
                    DialogueManager.StartDialogue(item.GetName(), pickupMessage);
                    FactManager.SetFact(item.Fact, true);
                }
            }
        }

        private void OnValidate() => gameObject.GetChildObject("ItemSprite").GetComponent<SpriteRenderer>().sprite = item.inventorySprite;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                DialogueManager.ShowPrompt("Press E to collect " + item.GetName());
                _inRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                DialogueManager.HidePrompt();
                _inRange = false;
            }
        }
    }
}

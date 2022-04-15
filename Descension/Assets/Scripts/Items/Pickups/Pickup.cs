using Actor.Interface;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Helpers;

namespace Items.Pickups
{
    public class Pickup : AInteractable
    {
        public EquippableItem item;
        public int quantity = 1;
        public string[] pickupMessage;
        public bool autoPickup;

        public void TryPickup()
        {
            if (!InventoryManager.PickupItem(item, ref quantity))
            {
                SoundManager.Error(); //TODO fail to pick up sound
                UIManager.GetHudController().ShowDialogue("Inventory full");
                return;
            }
            
            SoundManager.ItemFound();
                
            if (quantity == 0) Destroy(gameObject);
                
            // only show pickup dialogue once
            if (!FactManager.IsFactTrue(item.Fact))
            {
                DialogueManager.StartDialogue(item.GetName(), pickupMessage);
                FactManager.SetFact(item.Fact, true);
            }
        }

        private void OnValidate() => gameObject.GetChildObject("ItemSprite").GetComponent<SpriteRenderer>().sprite = item.inventorySprite;

        public override void Interact() => TryPickup();
        public override Vector2 Location() => gameObject.transform.position;
        public override string GetPrompt() => "Press F to pick up " + item.GetName();
    }
}

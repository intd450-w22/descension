using System.Xml;
using Actor.Interface;
using Actor.Player;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.Helpers;

namespace Items.Pickups
{
    public class Pickup : AInteractable, IUnique
    {
        [SerializeField] private int uniqueId;
        public int GetUniqueId() => uniqueId;
        public void SetUniqueId(int id) => uniqueId = id;
        
        public EquippableItem item;
        public int quantity = 1;
        public string[] pickupMessage;
        public bool autoPickup;
        public bool spawned = true;

        protected void Awake()
        {
            if (GameManager.IsUniqueDestroyed(this)) Destroy(gameObject);
        }
        
        protected void OnEnable() 
        {
            if (GetUniqueId() == 0) GameManager.GenerateNewUniqueId(this);
        }
        
        private void Update(){}
        

        public void TryPickup()
        {
            if (!InventoryManager.PickupItem(item, ref quantity))
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
                    GameManager.DestroyUnique(this);
                    Destroy(gameObject);
                }
                SoundManager.Error(); //TODO fail to pick up sound
                UIManager.GetHudController().ShowDialogue("Inventory full");
                return;
            }
            
            SoundManager.ItemFound();

            if (quantity == 0)
            {
                GameManager.DestroyUnique(this);
                Destroy(gameObject);
            }
                
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

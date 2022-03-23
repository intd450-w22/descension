using System;
using Managers;
using UnityEngine;

namespace Items.Pickups
{
    public class ArrowsItem : EquippableItem
    {
        public static String Name = "Arrows";
        
        public override string GetName()
        {
            return Name;
        }

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity)
        {
            return new Arrows(slotIndex, quantity, maxQuantity, inventorySprite);
        }
    }
    
    // logic for arrows (only used as ammo for bow)
    [Serializable]
    public class Arrows : Equippable
    {
        private PlayerControls _playerControls;
        
        public Arrows(int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = GetName();
            
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }

        public override String GetName()
        {
            return ArrowsItem.Name;
        }
        
        public override void SpawnDrop()
        {
            ItemSpawner.Instance.DropItem(ItemSpawner.Instance.arrowsPickupPrefab, Quantity);
        }
        
        public override void Update()
        {
            // if player tries to execute, equip bow if we have one
            if (_playerControls.Default.Shoot.WasPressedThisFrame())
            {
                InventoryManager.EquipFirstSlottedItem(BowItem.Name, false);
            }
        }


    }
}

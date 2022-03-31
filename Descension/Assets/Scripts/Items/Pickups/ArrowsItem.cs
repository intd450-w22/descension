using System;
using Managers;
using UnityEngine;

namespace Items.Pickups
{
    public class ArrowsItem : EquippableItem
    {
        public static String Name = "Arrows";
        
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) 
            => new Arrows(slotIndex, quantity, maxQuantity, inventorySprite);
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
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite)
            => new Arrows(slotIndex, quantity, maxQuantity, sprite);

        public override String GetName() => ArrowsItem.Name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.ArrowsPrefab, GameManager.PlayerController.transform.position, Quantity);

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

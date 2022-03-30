using System;
using UnityEngine;

namespace Items.Pickups
{
    public class TriggerItem : EquippableItem
    {
        public static string Name = "Trigger";

        // [Header("Trigger")]
        
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity)
        {
            return new Trigger(slotIndex, quantity, maxQuantity, inventorySprite);
        }
    }
    
    
    // logic for explosives
    [Serializable]
    class Trigger : Equippable
    {
        private bool _execute;
        private PlayerControls _playerControls;

        public Trigger(int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = GetName();
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite)
        {
            return new Trigger(slotIndex, quantity, maxQuantity, sprite);
        }

        public override String GetName() => SwordItem.Name;

        public override void SpawnDrop() => ItemSpawner.Instance.DropItem(ItemSpawner.Instance.swordPickupPrefab, Quantity);

        public override void OnEquip() {}

        public override void OnUnEquip() {}

        public override void Update() => _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();

        public override void FixedUpdate()
        {
            //******** Try to Execute if key pressed *******//
            if (!_execute) return;
            _execute = false;
            
        }
    }
}
using System;
using UnityEngine;

namespace Items.Pickups
{
    public class TimerItem : EquippableItem
    {
        public static string Name = "Timer";

        [Header("Timer")]
        public float seconds = 10f;
        
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity)
        {
            return new Timer(seconds, slotIndex, quantity, maxQuantity, inventorySprite);
        }
    }
    
    
    // logic for explosives
    [Serializable]
    class Timer : Equippable
    {
        private float _elapsed;
        private float _seconds;
        private bool _execute;
        private PlayerControls _playerControls;

        public Timer(float seconds, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = GetName();
            _seconds = seconds;
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite)
        {
            return new Timer(_seconds, slotIndex, quantity, maxQuantity, sprite);
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
using System;
using Managers;
using UnityEngine;

namespace Items.Pickups
{
    public class HealthItem : EquippableItem
    {
        public static String Name = "Health Potion";

        [Header("Health Potion")]
        public float healAmount = 50f;

        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) 
            => new HealthPotion(healAmount, slotIndex, quantity, maxQuantity, inventorySprite);
        
    }

    // logic for health potion
    [Serializable]
    class HealthPotion : Equippable
    {
        private bool _execute;
        private PlayerControls _playerControls;
        private float _healAmount;

        public HealthPotion(float healAmount, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = GetName();
            _healAmount = healAmount;

            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite)
            => new HealthPotion(_healAmount, slotIndex, quantity, maxQuantity, sprite);

        public override String GetName() => HealthItem.Name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.HealthPrefab, GameManager.PlayerController.transform.position, Quantity);

        public override void Update() => _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();

        public override void FixedUpdate()
        {
            //******** Try to Execute if key pressed *******//
            if (!_execute) return;
            _execute = false;

            if (Quantity <= 0)
            {
                UIManager.GetHudController().ShowText("No Health Potions remaining!");
                return;
            }
            GameManager.PlayerController.HealDamage(_healAmount);
            SoundManager.Heal();
            --Quantity;
        }

    }
}


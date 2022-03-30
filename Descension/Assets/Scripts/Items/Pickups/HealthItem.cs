using System;
using Actor.AI;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;
using Util.Helpers;

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
        {
            return new HealthPotion(healAmount, slotIndex, quantity, maxQuantity, inventorySprite);
        }
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

        public override String GetName()
        {
            return HealthItem.Name;
        }

        public override void SpawnDrop()
        {
            ItemSpawner.Instance.DropItem(ItemSpawner.Instance.healthPickupPrefab, Quantity);
        }

        public override void Update()
        {
            if (GameManager.IsFrozen) return;

            _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();
        }

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
            //Todo Add sound
            //SoundManager.Heal();
            --Quantity;
        }

    }
}


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
        

        public override string GetName()
        {
            return Name;
        }

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity)
        {
            return new HealthPotion(slotIndex, quantity, maxQuantity, inventorySprite);
        }
    }

    // logic for health potion
    [Serializable]
    class HealthPotion : Equippable
    {
        public float healAmount = 50;
        private bool _execute;
        private PlayerControls _playerControls;

        public HealthPotion(int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = GetName();

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
            _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();
        }

        public override void FixedUpdate()
        {
            PlayerController controller = GameManager.PlayerController;
            Vector3 screenPoint = controller.playerCamera.WorldToScreenPoint(controller.transform.localPosition);
            Vector3 direction = (Input.mousePosition - screenPoint).normalized;
            Vector3 position = controller.transform.position;


            //******** Try to Execute if key pressed *******//
            if (!_execute) return;
            _execute = false;

            if (Quantity <= 0)
            {
                UIManager.GetHudController().ShowText("No Health Potions remaining!");
                return;
            }
            controller.GetComponent<PlayerController>().HealDamage(healAmount);
            //Todo Add sound
            //SoundManager.Heal();
            --Quantity;
        }

    }
}


using System;
using Actor.Player;
using Managers;
using UnityEngine;

namespace Items.Pickups
{
    public class HealthItem : EquippableItem
    {
        public static string Name = "Health Potion";

        [Header("Health Potion")]
        public float healAmount = 50f;

        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) => new HealthPotion(this, slotIndex, quantity);
    }

    // logic for health potion
    [Serializable]
    internal class HealthPotion : Equippable
    {
        // attributes
        private float _healAmount;
        
        public HealthPotion(HealthItem healthItem, int slotIndex, int quantity) : base(healthItem, slotIndex, quantity) 
            => Init(healthItem.healAmount);
        
        public HealthPotion(HealthPotion healthPotion) : base(healthPotion) 
            => Init(healthPotion._healAmount);
        
        public void Init(float healAmount)
        {
            name = HealthItem.Name;
            _healAmount = healAmount;
        }
        
        public override Equippable DeepCopy() => new HealthPotion(this);

        public override string GetName() => name;

        public override void SpawnDrop() => SpawnManager.SpawnItem(SpawnManager.HealthPrefab, PlayerPosition, Quantity);

        public override void Execute()
        {
            base.Execute();

            if (Quantity <= 0)
            {
                DialogueManager.ShowPrompt("No Health Potions remaining!");
                return;
            }
            PlayerController.Instance.HealDamage(_healAmount);
            SoundManager.Heal();
            --Quantity;
        }

    }
}


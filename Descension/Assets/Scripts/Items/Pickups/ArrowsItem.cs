using System;
using Actor.Player;
using Managers;
using UnityEngine;

namespace Items.Pickups
{
    public class ArrowsItem : EquippableItem
    {
        public static String Name = "Arrows";
        
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) => new Arrows(this, slotIndex, quantity);
    }
    
    // logic for arrows (only used as ammo for bow)
    [Serializable]
    internal class Arrows : Equippable
    {
        public Arrows(ArrowsItem arrowsItem, int slotIndex, int quantity) : base(arrowsItem, slotIndex, quantity) => Init();
        
        public Arrows(Arrows arrows) : base(arrows) => Init();
        
        private void Init() => name = ArrowsItem.Name;

        public override Equippable DeepCopy() => new Arrows(this);

        public override String GetName() => name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.ArrowsPrefab, PlayerPosition, Quantity);
        
        protected override void Execute() => InventoryManager.EquipFirstSlottedItem(BowItem.Name, false);
    }
}

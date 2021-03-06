using System;
using Managers;

namespace Actor.Items.Pickups
{
    public class ArrowsItem : EquippableItem
    {
        public static string Name = "Arrows";
        
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

        public override string GetName() => name;

        public override void SpawnDrop() => SpawnManager.SpawnItem(SpawnManager.ArrowsPrefab, PlayerPosition, Quantity);
        
        public override void Execute() => InventoryManager.EquipFirstSlottedItem(BowItem.Name, false);
    }
}

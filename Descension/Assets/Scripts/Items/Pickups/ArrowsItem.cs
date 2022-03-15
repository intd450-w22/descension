using System;
using UnityEngine;

namespace Items.Pickups
{
    public class ArrowsItem : EquippableItem
    {
        public static String Name = "Arrows";

        public GameObject arrowPrefab;
        public float bowReticleDistance = 2f;
        
        public override string GetName()
        {
            return Name;
        }

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance()
        {
            return new Arrows();
        }
    }
    
    // logic for arrows (only used as ammo for bow)
    [Serializable]
    public class Arrows : Equippable
    {
        public Arrows()
        {
            name = GetName();
        }

        public override String GetName()
        {
            return ArrowsItem.Name;
        }
        
        public override void OnDrop()
        {
            ItemSpawner.Instance.DropItem(ItemSpawner.Instance.arrowsPickupPrefab, durability);
            base.OnDrop();
        }
    }
}

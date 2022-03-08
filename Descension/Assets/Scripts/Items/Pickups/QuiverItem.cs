using System;
using Actor.Player;
using Managers;
using UnityEngine;

namespace Items.Pickups
{
    public class QuiverItem : EquippableItem
    {
        const String Name = "Quiver";

        public GameObject arrowPrefab;
        public float bowReticleDistance = 2f;
        
        public override string GetName()
        {
            return Name;
        }

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance()
        {
            return new Quiver();
        }
    }
    
    // logic for bow
    [Serializable]
    public class Quiver : Equippable
    {
        public Quiver()
        {
            name = GetName();
        }

        public String GetName()
        {
            return "Quiver";
        }
        
        public override void OnDrop()
        {
            ItemSpawner.Instance.DropItem(ItemSpawner.Instance.QuiverPickupPrefab, durability);
            base.OnDrop();
        }
    }
}

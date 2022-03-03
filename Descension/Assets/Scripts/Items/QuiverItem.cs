using System;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;
using Util.Helpers;


namespace Items
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
        public override Equippable CreateInstance(InventoryManager manager, PlayerController controller, int durability)
        {
            return new Quiver(controller, durability);
        }
    }
    
    // logic for bow
    [Serializable]
    public class Quiver : Equippable
    {
        private PlayerController _controller;

        public Quiver(PlayerController controller, int durability)
        {
            _controller = controller;
            this.durability = durability;
            this.name = GetName();
        }

        public String GetName()
        {
            return "Quiver";
        }
    }
}

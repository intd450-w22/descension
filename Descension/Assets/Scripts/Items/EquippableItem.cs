using System;
using Actor.Player;
using JetBrains.Annotations;
using Managers;
using UnityEngine;

namespace Items
{
    // Inspector compatible component for creating pickups
    [Serializable]
    public abstract class EquippableItem : MonoBehaviour
    {
        
        public PlayerController controller;
        public Sprite inventorySprite;  // sprite displayed in inventory
        public int quantity = 0;
        

        public abstract String GetName();
        
        // should create instance of Equippable child
        public abstract Equippable CreateInstance(InventoryManager manager, PlayerController controller, int durability = 1);
        
        // // Called when action button is pressed and item is equipped
        // public abstract void Use(ref int durability);
    }
    
    // actual object with functionality
    [Serializable, CanBeNull]
    public class Equippable
    {
        public String name;
        public int durability = -1;
        
        public virtual String GetName() { return name; }

        public virtual void Update() {}

        public virtual void FixedUpdate() { }
    }
}

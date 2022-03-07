using System;
using Actor.Player;
using JetBrains.Annotations;
using Managers;
using UnityEngine;

namespace Items.Pickups
{
    // Inspector compatible component for creating pickups
    [Serializable]
    public abstract class EquippableItem : MonoBehaviour
    {
        
        [HideInInspector] public PlayerController controller;
        public Sprite inventorySprite;  // sprite displayed in inventory
        public int quantity = 0;
        

        public abstract String GetName();
        
        // should create instance of Equippable child
        public abstract Equippable CreateInstance();
        
        // // Called when action button is pressed and item is equipped
        // public abstract void Use(ref int durability);
    }
    
    // actual object with functionality
    [Serializable, CanBeNull]
    public class Equippable
    {
        public String name;
        public int durability = -1;
        
        // initialize references, need to do this whenever scene changes
        public virtual void Initialize() {}
        
        // should return name of equippable item
        public virtual String GetName() { return name; }

        // called when equipped switches to different slot (equippable)
        public virtual void OnEquip() { }
        
        // called when equipped switches to this slot (equippable)
        public virtual void OnUnEquip() { }

        
        // called like regular MonoBehavior Update() if this item is equipped
        public virtual void Update() {}

        // called like regular MonoBehavior FixedUpdate() if this item is equipped
        public virtual void FixedUpdate() { }

        public void SetDurability(int durability) { this.durability = durability; }
    }
}

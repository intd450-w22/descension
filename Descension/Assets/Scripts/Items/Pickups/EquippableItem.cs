using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Items.Pickups
{
    // Inspector compatible component for creating pickups
    [Serializable]
    public abstract class EquippableItem : MonoBehaviour
    {
        // sprite displayed in inventory UI
        public Sprite inventorySprite;

        public abstract String GetName();
        
        // should create instance of Equippable
        public abstract Equippable CreateInstance();
    }
    
    // actual object with functionality
    [Serializable, CanBeNull]
    public class Equippable
    {
        public String name;
        public int quantity = -1;
        [HideInInspector] public Sprite inventorySprite;
        
        public delegate void OnQuantityUpdatedDelegate(int newDurability);
        public OnQuantityUpdatedDelegate OnQuantityUpdated;

        // initialize references, need to do this whenever scene changes
        public virtual void Initialize() {}
        
        // should return name of equippable item
        public virtual String GetName() { return name; }

        // called when equipped switches to different slot (equippable)
        public virtual void OnEquip() { }
        
        // called when equipped switches from this slot (equippable)
        public virtual void OnUnEquip() { }
        
        // called when item is dropped
        public virtual void OnDrop()
        {
            name = "";
            quantity = -1;
            inventorySprite = null;
        }
        
        // called like regular MonoBehavior Update() if this item is equipped
        public virtual void Update() {}

        // called like regular MonoBehavior FixedUpdate() if this item is equipped
        public virtual void FixedUpdate() { }

        // sets durability/quantity for this item
        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
            OnQuantityUpdated?.Invoke(quantity);
        }
    }
}

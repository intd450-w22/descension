using System;
using JetBrains.Annotations;
using Managers;
using UnityEngine;

namespace Items.Pickups
{
    // Inspector compatible component for creating pickups
    [Serializable]
    public abstract class EquippableItem : MonoBehaviour
    {
        // sprite displayed in inventory UI
        public Sprite inventorySprite;
    
        // the maximum quantity/durability for this item
        public int maxQuantity = 30;

        public abstract String GetName();
        
        // should create instance of Equippable
        public abstract Equippable CreateInstance(int slotIndex, int quantity);
    }
    
    // actual object with functionality, would be abstract but then it doesn't display in inspector
    [Serializable, CanBeNull]
    public class Equippable
    {
        public Equippable(int slotIndex, int quantity, int maxQuantity, Sprite sprite)
        {
            _slotIndex = slotIndex;
            _maxQuantity = maxQuantity;
            
            Quantity = quantity;
            inventorySprite = sprite;
        }
        
        public String name;
        [HideInInspector] public Sprite inventorySprite;
        [SerializeField] private int _quantity = 0;
        private int _maxQuantity;
        private int _slotIndex;

        // quantity/durability for item
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = Math.Min(value, _maxQuantity);

                if (_quantity <= 0) InventoryManager.DropSlot(_slotIndex);  // auto drop from slot if quantity/durability hits 0
                else OnQuantityUpdated?.Invoke(_quantity);  // update UI
            }
        }
        
        public delegate void OnQuantityUpdatedDelegate(int newDurability);
        public OnQuantityUpdatedDelegate OnQuantityUpdated;

        // return name of equippable item
        public virtual String GetName() { return name; }
        
        // should override to spawn pickup item
        public virtual void SpawnDrop() { Debug.LogWarning("SpawnDrop() should be overriden in " + this); }

        // returns the max quantity/durability for this item
        public int GetMaxQuantity()
        {
            return _maxQuantity;
        }
        
        // removes data from class instance and UI
        public void Clear()
        {
            name = "";
            _quantity = -1;
            inventorySprite = null;
            UIManager.Hotbar.DropItem(_slotIndex);
        }

        // called when equipped switches to different slot (equippable)
        public virtual void OnEquip() { }
        
        // called when equipped switches from this slot (equippable)
        public virtual void OnUnEquip() { }
        
        // called when item is dropped
        public void OnDrop()
        {
            SpawnDrop();
            Clear();
        }

        // called like regular MonoBehavior Update() if this item is equipped
        public virtual void Update() {}

        // called like regular MonoBehavior FixedUpdate() if this item is equipped
        public virtual void FixedUpdate() { }
    }
}

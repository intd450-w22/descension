using System;
using Actor.Player;
using Managers;
using UnityEngine;
using UnityEngine.Assertions;
using Util.Enums;
using Util.Helpers;

namespace Items.Pickups
{
    // Inspector compatible component for creating pickups
    [Serializable]
    public abstract class EquippableItem : MonoBehaviour
    {
        [Header("Equippable")]
        // sprite displayed in inventory UI
        public Sprite inventorySprite;
    
        // the maximum quantity/durability for this item
        public int maxQuantity = 30;

        // minimum time between executions
        public float cooldownTime;
        
        public abstract string GetName();

        public FactKey Fact;
        
        // should create instance of Equippable
        public abstract Equippable CreateInstance(int slotIndex, int quantity);
    }
    
    // actual object with functionality, would be abstract but then it doesn't display in inspector
    [Serializable]
    public class Equippable
    {
        // attributes
        public string name = "";
        [SerializeField] private int _quantity;
        private int _maxQuantity;
        private int _slotIndex;
        private float _cooldownTime;
        [HideInInspector] public Sprite inventorySprite;
        
        // state
        private float _cooldown;
        private bool _execute;
        private static PlayerControls _playerControls;
        
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

        private Transform _reticle;
        protected Transform Reticle => _reticle ??= PlayerController.Reticle;

        private Transform _spriteTransform;
        protected Transform SpriteTransform => _spriteTransform ??= PlayerController.ItemObject;
        protected Sprite Sprite { set => PlayerController.ItemSprite = value; }

        private Transform _playerTransform;
        protected Transform PlayerTransform => _playerTransform ??= PlayerController.Instance.transform;
        protected Vector3 PlayerPosition => PlayerTransform.position;
        
        public delegate void OnQuantityUpdatedDelegate(int newDurability);
        public OnQuantityUpdatedDelegate OnQuantityUpdated;
        
        // do not use
        public Equippable() { 
            name = "";
            _quantity = -1;
            _slotIndex = -1;
        }
        
        public Equippable(EquippableItem item, int slotIndex, int quantity) :
            this(slotIndex, item.maxQuantity, quantity, item.cooldownTime, item.inventorySprite) {}

        public Equippable(Equippable equippable) :
            this(equippable._slotIndex, equippable._maxQuantity, equippable._quantity, equippable._cooldownTime, equippable.inventorySprite) {}

        private Equippable(int slotIndex, int maxQuantity, int quantity, float cooldownTime, Sprite sprite)
        {
            Assert.IsTrue(slotIndex < InventoryManager.Slots.Count, "slotIndex " + slotIndex + " given in Equippable constructor, must be less than " + InventoryManager.Slots.Count);
            
            _slotIndex = slotIndex;
            _maxQuantity = maxQuantity;
            
            Quantity = quantity;
            inventorySprite = sprite;

            _cooldownTime = cooldownTime;

            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();
                _playerControls.Enable();
            }
        }

        public virtual Equippable DeepCopy() => new Equippable(this);

        // return name of equippable item
        public virtual string GetName() { return name; }
        
        // should override to spawn pickup item
        public virtual void SpawnDrop() { GameDebug.LogWarning("SpawnDrop() should be overriden in " + this); }

        // returns the max quantity/durability for this item
        public int GetMaxQuantity() => _maxQuantity;

        // removes data from class instance and UI
        public void Clear()
        {
            name = "";
            _quantity = -1;
            inventorySprite = null;
            UIManager.Hotbar.DropItem(_slotIndex);
        }

        // called when equipped switches to different slot (equippable)
        public virtual void OnEquip()
        {
            Sprite = inventorySprite;
            SpriteTransform.gameObject.SetActive(true);
            SpriteTransform.SetPositionAndRotation(PlayerPosition + new Vector3(2, 0,0), Quaternion.identity);
        }
        
        // called when equipped switches from this slot (equippable)
        public virtual void OnUnEquip() => SpriteTransform.gameObject.SetActive(false);

        // called when item is dropped
        public void OnDrop()
        {
            SpawnDrop();
            OnUnEquip();
            Clear();
        }

        // called like regular MonoBehavior Update() if this item is equipped
        public void Update() => _execute |= _playerControls.Default.Shoot.WasPressedThisFrame() && Time.time >= _cooldown;

        // called like regular MonoBehavior FixedUpdate() if this item is equipped
        public void EquippedFixedUpdate()
        {
            FixedUpdate();
            
            if (!_execute) return;
            
            _execute = false;
            _cooldown = Time.time + _cooldownTime;
            Execute();
        }
        
        // called like regular MonoBehavior FixedUpdate() if this item is equipped
        protected virtual void FixedUpdate() {}
        
        // called when execute button is pressed and cooldown has finished if this item is equipped
        protected virtual void Execute() {}
    }
}

using System;
using System.Collections.Generic;
using Actor.Items.Pickups;
using Actor.Player;
using UnityEngine;
using Util.EditorHelpers;
using Util.Helpers;
using static Util.Helpers.CalculationHelper;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<Equippable> slots = new List<Equippable>() { new Equippable(), new Equippable(), new Equippable(), new Equippable() };
        [SerializeField, ReadOnly] private List<Equippable> cachedSlots = new List<Equippable>() { new Equippable(), new Equippable(), new Equippable(), new Equippable() };
        [SerializeField] private int equippedSlot = -1;
        [SerializeField] private int gold = 0;

        [SerializeField] private float globalCooldown = .01f;
        [SerializeField] private int updateInterval = 3;
        private int _updateCount = 0;
        private float _cooldown = 0f;
        
        private static InventoryManager _instance;
        private static InventoryManager Instance => _instance ??= FindObjectOfType<InventoryManager>();

        public static List<Equippable> Slots => Instance.slots;
        private static List<Equippable> CachedSlots => Instance.cachedSlots;
        
        public static int Gold
        {
            get => Instance.gold;
            set => Instance.gold = value;
        }

        public static void OnSceneComplete() => CacheSlots();
        
        // called when a scene is loaded
        public static void OnReloadScene() => Instance._OnReloadScene();
        private void _OnReloadScene()
        {
            // start with cached items
            LoadCachedSlots();
            for (var i = 0; i < Slots.Count; ++i) UIManager.Hotbar.PickupItem(Slots[i], i);
            if (equippedSlot != -1) EquipSlot(equippedSlot);
            else EquipFirstSlottedItem();
        }

        private static void CacheSlots()
        {
            for (var i = 0; i < Slots.Count; ++i) CachedSlots[i] = Slots[i].DeepCopy();
        }
        
        private static void LoadCachedSlots()
        {
            ClearSlots();
            for (var i = 0; i < Slots.Count; ++i) Slots[i] = CachedSlots[i].DeepCopy();
        }

        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this)
            {
                Destroy(gameObject);
                CacheSlots();
            }
        }

        void Update()
        {
            if (GameManager.IsFrozen) return;

            // check for equipped item slot change
            int scroll;
            if (equippedSlot != -1 && (scroll = (int) Input.mouseScrollDelta.y) != 0)
            {
                int i = equippedSlot;
                while (slots[i = SafeIndex(i-scroll, slots.Count)].Quantity <= 0) {}
                if (i != equippedSlot) EquipSlot(i);
            }
        }
        
        void FixedUpdate()
        {
            if (GameManager.IsFrozen || ++_updateCount % updateInterval != 0) return;
            
            // run logic for equipped weapon
            if (equippedSlot != -1) slots[equippedSlot].EquippedFixedUpdate();
        }
        
        // inventory logic for when player is killed
        public static void OnKilled() => Instance._OnKilled();
        private void _OnKilled()
        {
            PlayerController.Reticle.gameObject.SetActive(false);
            PlayerController.SpriteTransform.DetachChildren();  // visually drop weapon
        
            // reattach when movement stops
            this.InvokeWhen(
                ()=>PlayerController.ItemObject.SetParent(PlayerController.SpriteTransform),
                ()=>PlayerController.Velocity.sqrMagnitude < 1, 
                0.5f);
        }
        
        public static void TryEquipSlot(int index) => Instance._TryEquipSlot(index);
        private void _TryEquipSlot(int index)
        {
            if(slots[index].Quantity >= 0) EquipSlot(index);
        }

        // sets item at index to equipped state
        void EquipSlot(int index)
        {
            if (equippedSlot != -1 && slots[equippedSlot] != null) slots[equippedSlot].OnUnEquip();
            if (slots[index]?.Quantity != -1)
            {
                SoundManager.SwitchItem();
                equippedSlot = index;
                slots[equippedSlot].OnEquip();
                UIManager.Hotbar.SetActive(index);
                GameDebug.Log("Slot " + index + " equipped / " + slots[equippedSlot]);
            }
        }
        
        // find first slot with equippable item and set it to equipped state
        public static void EquipFirstSlottedItem() => Instance._EquipFirstSlottedItem();
        void _EquipFirstSlottedItem()
        {
            for (int i = 0; i < slots.Count; ++i)
            {
                if (slots[i]?.Quantity != -1)
                {
                    EquipSlot(i);
                    return;
                }
            }
            
            equippedSlot = -1;
        }
        
        // find first slot with name and set it to equipped state, otherwise find first slot with equippable item and set it to equipped state if defaultAny=true
        public static void EquipFirstSlottedItem(string itemName, bool defaultAny = true) => Instance._EquipFirstSlottedItem(itemName, defaultAny);
        private void _EquipFirstSlottedItem(string itemName, bool defaultAny = true)
        {
            for (int i = 0; i < slots.Count; ++i)
            {
                if (slots[i]?.name == itemName)
                {
                    EquipSlot(i);
                    return;
                }
            }

            // equip first item if this fails
            if (defaultAny) EquipFirstSlottedItem();
        }
        
        // drop the item that is currently equipped
        public static void DropCurrentSlot() => Instance._DropCurrentSlot();
        private void _DropCurrentSlot() => _DropSlot(equippedSlot);

        // drop item at slot index and update UI
        public static void DropSlot(int index) => Instance._DropSlot(index);
        private void _DropSlot(int index)
        {
            GameDebug.Log("DropSlot(" + index + ")");
            if (index != -1)
            {
                var itemName = slots[index].name;
                slots[index].OnDrop();
                slots[index] = new Equippable();

                // equip item with same name if possible if this was an executable item (not ammunition)
                if (index == equippedSlot) EquipFirstSlottedItem(itemName);
            }
        }

        // add item to inventory if room, quantity will be updated relative to how many items are remaining in the pickup if the inventory is full
        // returns false if no item quantity/durability could be picked up
        public static bool PickupItem(EquippableItem item, ref int quantity) => Instance._PickupItem(item, ref quantity);
        private bool _PickupItem(EquippableItem item, ref int quantity)
        {
            // torch custom logic
            if (item.name == TorchItem.Name)
            {
                PlayerController.AddTorch(quantity);
                quantity = 0;
                return true;
            }
            
            
            int initialQuantity = quantity;

            // add durability/quantity if already have this item
            for (int i = 0; i < slots.Count && quantity > 0; ++i)
            {
                if (slots[i].name == item.GetName())
                {
                    var extra = Math.Max(0, quantity - (slots[i].GetMaxQuantity() - slots[i].Quantity));
                    slots[i].Quantity += quantity;
                    quantity = extra;
                }
            }
            
            
            // add to empty slot if one is available
            for (int i = 0; i < slots.Count && quantity > 0; ++i)
            {
                if (slots[i].name.Length == 0)
                {
                    slots[i] = item.CreateInstance(i, quantity);
                    quantity -= slots[i].Quantity;
                    UIManager.Hotbar.PickupItem(slots[i], i);
                    
                    if (equippedSlot == -1) EquipSlot(i); // auto equip if we have nothing equipped
                }
            }

            if (quantity == initialQuantity)
            {
                SwapEquipped(item, ref quantity);
                return true;
            }
            // returns false only if no items were picked up
            return quantity != initialQuantity;
        }

        // drops currently equipped item and picks up item
        private static void SwapEquipped(EquippableItem item, ref int quantity) => Instance._SwapEquipped(item, ref quantity);
        void _SwapEquipped(EquippableItem item, ref int quantity)
        {
            int slot = equippedSlot;
            DropSlot(equippedSlot);
            PickupItem(item, ref quantity);
            EquipSlot(slot);
            quantity = 0;
        }
        
        // remove item from slot and update UI
        private void ClearSlot(int slotIndex)
        {
            slots[slotIndex].Quantity = -1;
        }

        public static void ResetInventory()
        {
            ClearSlots();
            ClearCachedSlots();
            Gold = 0;
            Instance.equippedSlot = -1;
        }

        // remove all items from slots and update UI
        private static void ClearSlots() => Instance._ClearSlots();
        private void _ClearSlots()
        {
            for (var i = 0; i < slots.Count; ++i) ClearSlot(i);
        }
        
        private static void ClearCachedSlots() => Instance._ClearSlots();
        private void _ClearCachedSlots()
        {
            for (var i = 0; i < cachedSlots.Count; ++i) cachedSlots[i] = new Equippable();
        }

        public static void SetCooldown() => Instance._SetCooldown();
        private void _SetCooldown() => _cooldown = Time.time + globalCooldown;

        public static bool IsOnCooldown() => Instance._IsOnCooldown();
        private bool _IsOnCooldown() => Time.time < _cooldown;

        public static void TryExecute() => Instance._TryExecute();

        private void _TryExecute()
        {
            if (_IsOnCooldown() || !IsInRange(equippedSlot, slots.Count) || slots[equippedSlot].IsOnCooldown()) return;
            slots[equippedSlot].Execute();
        }

    }
}

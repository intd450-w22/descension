using System;
using System.Collections.Generic;
using System.Linq;
using Items.Pickups;
using UnityEngine;
using Util.Helpers;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<Equippable> slots = new List<Equippable>() { null, null, null, null };
        [SerializeField] private int equippedSlot = -1;
        [SerializeField] private int gold = 0;
        
        private static InventoryManager _instance;
        private static InventoryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InventoryManager>();
                }
                return _instance;
            }
        }

        public static List<Equippable> Slots => Instance.slots;
        
        public static int Gold
        {
            get => Instance.gold;
            set => Instance.gold = value;
        }
        
        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            // check for equipped item slot change
            if      (Input.GetKeyDown(KeyCode.Alpha1) && slots[0].Quantity >= 0) EquipSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2) && slots[1].Quantity >= 0) EquipSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3) && slots[2].Quantity >= 0) EquipSlot(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4) && slots[3].Quantity >= 0) EquipSlot(3);

            // run logic for equipped item
            if (equippedSlot != -1) slots[equippedSlot].Update();
            
            // drop equipped item on R
            if (Input.GetKeyDown(KeyCode.R)) DropSlot(equippedSlot);
        }
        
        void FixedUpdate()
        {
            // run logic for equipped weapon
            if (equippedSlot != -1) slots[equippedSlot].FixedUpdate();
        }
        
        // inventory logic for when player is killed
        public static void OnKilled() => Instance._OnKilled();
        private void _OnKilled()
        {
            ClearSlots();
        }
        
        // sets item at index to equipped state
        void EquipSlot(int index)
        {
            if (equippedSlot != -1 && slots[equippedSlot] != null) slots[equippedSlot].OnUnEquip();
            if (slots[index]?.Quantity != -1)
            {
                equippedSlot = index;
                slots[equippedSlot].OnEquip();
                UIManager.Hotbar.SetActive(index);
                Debug.Log("Slot " + index + " equipped / " + slots[equippedSlot]);
            }
        }
        
        // find first slot with equippable item and set it to equipped state
        void EquipFirstSlottedItem()
        {
            for (int i = 0; i < slots.Count; ++i)
            {
                if (slots[i].Quantity != -1)
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
                if (slots[i] != null && slots[i].name == itemName)
                {
                    EquipSlot(i);
                    return;
                }
            }

            // equip first item if this fails
            if (defaultAny) EquipFirstSlottedItem();
        }
        
        // drop item at slot index and update UI
        public static void DropSlot(int index) => Instance._DropSlot(index);
        private void _DropSlot(int index)
        {
            if (index != -1)
            {
                var itemName = slots[index].name;
                slots[index].OnDrop();

                // equip item with same name if possible if this was an executable item (not ammunition)
                if (index == equippedSlot) EquipFirstSlottedItem(itemName);
            }
        }

        // add item to inventory if room, quantity will be updated relative to how many items are remaining in the pickup if the inventory is full
        // returns false if no item quantity/durability could be picked up
        public static bool PickupItem(EquippableItem item, ref int quantity) => Instance._PickupItem(item, ref quantity);
        private bool _PickupItem(EquippableItem item, ref int quantity)
        {
            int initialQuantity = quantity;
            FactManager.SetFact(item.Fact, true);

            // keep code in case we dont want multiple slots of same item
            // // add durability/quantity if already have this item
            // var inventoryItem = slots.SingleOrDefault(x => x.name == item.GetName());
            // if (inventoryItem != null && inventoryItem.Quantity < inventoryItem.GetMax())
            // {
            //     extra = quantity - (inventoryItem.GetMax() - inventoryItem.Quantity);
            //     inventoryItem.Quantity += quantity;
            //     if (extra == quantity)
            //     {
            //         Debug.Log("Pickup Failed, Item at Max");
            //         return false;
            //     }
            //     Debug.Log("Pickup Success");
            //     return true;
            // }
            
            // add durability/quantity if already have this item
            for (int i = 0; i < slots.Count && quantity > 0; ++i)
            {
                if (slots[i].name == item.GetName())
                {
                    var extra = Math.Max(0, quantity - (slots[i].GetMaxQuantity() - slots[i].Quantity));
                    slots[i].Quantity += quantity;
                    quantity = extra;
                    Debug.Log("Extra " + extra);
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
            
            Debug.Log("Remaining Extra " + quantity);
            // returns false only if no items were picked up
            return quantity != initialQuantity;
        }
        
        // remove item from slot and update UI
        void ClearSlot(int slotIndex)
        {
            slots[slotIndex].Quantity = -1;
        }
        
        // remove all items from slots and update UI
        void ClearSlots()
        {
            for (int i = 0; i < slots.Count; ++i) ClearSlot(i);
        }
        

    }
}

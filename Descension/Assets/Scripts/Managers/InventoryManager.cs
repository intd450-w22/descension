using System;
using System.Collections.Generic;
using System.Linq;
using Actor.Player;
using Items;
using Items.Pickups;
using JetBrains.Annotations;
using UI.Controllers;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        private static InventoryManager _instance;
        
        public static InventoryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InventoryManager>();
                }
                return _instance;
            }
            set => _instance = value;
        }
        
        public List<Equippable> slots = new List<Equippable>() { null, null, null };
        public int equippedSlot = -1;
        public int gold = 0;

        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            // check for equipped item slot change
            if      (Input.GetKeyDown(KeyCode.Alpha1) && slots[0].durability >= 0) EquipSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2) && slots[1].durability >= 0) EquipSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3) && slots[2].durability >= 0) EquipSlot(2);

            // run logic for equipped item
            if (equippedSlot != -1) slots[equippedSlot].Update();
            
            // drop equipped item on R
            if (Input.GetKeyDown(KeyCode.R) && slots[0].durability >= 0) DropSlot(equippedSlot);
        }
        
        void FixedUpdate()
        {
            // run logic for equipped weapon
            if (equippedSlot != -1) slots[equippedSlot].FixedUpdate();
        }

        void EquipSlot(int index)
        {
            if (equippedSlot != -1 && slots[equippedSlot] != null) slots[equippedSlot].OnUnEquip();
            equippedSlot = index;
            slots[equippedSlot].OnEquip();
            UIManager.Instance.Hotbar.SetActive(index);
        }

        void EquipFirstSlottedItem()
        {
            for (int i = 0; i < slots.Count; ++i)
            {
                if (slots[i].durability != -1)
                {
                    EquipSlot(i);
                    Debug.Log("Equipped " + i);
                    return;
                }
            }

            equippedSlot = -1;
        }

        void DropSlot(int index)
        {
            if (index != -1)
            {
                slots[index].OnDrop();
                EquipFirstSlottedItem();
                UIManager.Instance.Hotbar.DropItem(index);
            }
        }

        public bool PickupItem(EquippableItem item, int quantity)
        {
            // add durability/quantity if already have this item
            var inventoryItem = slots.SingleOrDefault(x => x.name == item.GetName());
            if (inventoryItem != null)
            {
                inventoryItem.durability += quantity;
                Debug.Log("Pickup Success");
                return true;
            }

            // add to empty slot otherwise if one is available
            for (int i = 0; i < slots.Count; ++i)
            {
                if (slots[i].name.Length == 0)
                {
                    slots[i] = item.CreateInstance();
                    slots[i].SetDurability(quantity);
                    slots[i].inventorySprite = item.inventorySprite;
                    EquipSlot(i);
                    UIManager.Instance.Hotbar.PickupItem(item, i);
                    Debug.Log("Pickup Success");
                    return true;
                }
            }
            
            Debug.Log("Pickup Failed");

            return false;
        }
        
    }
}

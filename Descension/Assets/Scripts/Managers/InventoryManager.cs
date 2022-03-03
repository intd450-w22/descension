using System;
using System.Collections.Generic;
using Actor.Player;
using Items;
using JetBrains.Annotations;
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
                if (_instance == null) _instance = FindObjectOfType<InventoryManager>();
                return _instance;
            }
            set => _instance = value;
        }
        
        
        public PlayerController controller;
        public List<Equippable> slots = new List<Equippable>() { null, null, null };
        public int equippedSlot = -1;

        private PlayerControls _playerControls;
        private bool _execute;

        void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("");
            }

            controller = player.GetComponent<PlayerController>();
            _playerControls = controller.playerControls;
        }

        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (equippedSlot != -1) slots[equippedSlot].Update();
        }
        
        void FixedUpdate()
        {
            if (equippedSlot != -1) slots[equippedSlot].FixedUpdate();
        }

        public bool PickupItem(EquippableItem item, int quantity)
        {
            for (int i = 0; i < slots.Count; ++i)
            {
                if (slots[i].GetName() == item.GetName())
                {
                    slots[i].durability += quantity;
                    Debug.Log("Pickup Success");
                    return true;
                }
            }
            
            for (int i = 0; i < slots.Count; ++i)
            {
                if (slots[i].GetName().Length == 0)
                {
                    slots[i] = item.CreateInstance(this, controller, quantity);
                    equippedSlot = i;
                    Debug.Log("Pickup Success");
                    return true;
                }
            }
            
            Debug.Log("Pickup Failed");

            return false;
        }
        
    }
}

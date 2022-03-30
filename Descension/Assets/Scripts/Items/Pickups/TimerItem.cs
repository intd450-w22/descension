using System;
using Managers;
using UnityEngine;

namespace Items.Pickups
{
    public class TimerItem : EquippableItem
    {
        public static string Name = "Timer";

        [Header("Timer")] 
        public string activationMessage = "Timer is ticking down!";
        
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity)
        {
            return new Timer(activationMessage, slotIndex, quantity, maxQuantity, inventorySprite);
        }
    }
    
    
    // logic for explosives
    [Serializable]
    class Timer : Equippable
    {
        private string _activationMessage;
        private float _seconds;
        private bool _execute;
        private bool _started;
        private PlayerControls _playerControls;

        public Timer(string activationMessage, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = GetName();
            _activationMessage = activationMessage;
            _seconds = maxQuantity;
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite)
        {
            return new Timer(_activationMessage, slotIndex, quantity, maxQuantity, sprite);
        }

        public override String GetName() => TimerItem.Name;

        public override void SpawnDrop() => ItemSpawner.Instance.DropItem(ItemSpawner.Instance.timerPickupPrefab, Quantity);

        public override void OnEquip() {}

        public override void OnUnEquip() {}
        
        public override void Update()
        {
            _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();
        }

        public override void FixedUpdate()
        {
            if (GameManager.IsFrozen) return;
            
            // can only start once
            if (!_execute) return;
            if (!_started)
            {
               DialogueManager.ShowNotification(_activationMessage);
               _started = true;
            }
            
            int intSeconds = (int) (_seconds -= Time.deltaTime);
            if (intSeconds == 0)
            {
                // TODO What happens when the timer runs out?
                Debug.Log("TIMER UP!");
            }
            if (Quantity != intSeconds)
            {
                Quantity = intSeconds;
            }
        }
    }
}
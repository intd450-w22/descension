using System;
using Environment;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Items.Pickups
{
    public class TriggerItem : EquippableItem
    {
        public static string Name = "Trigger";

        [Header("Trigger")] 
        public string outOfRangeMessage = "Trigger added to bomb!";
        public string addToBombMessage = "Need to add this to the bomb.";
        
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) 
            => new Trigger(outOfRangeMessage, addToBombMessage, slotIndex, quantity, maxQuantity, inventorySprite);
    }
    
    
    // logic for explosives
    [Serializable]
    class Trigger : Equippable
    {
        private string _outOfRangeMessage;
        private string _addToBombMessage;
        private bool _execute;
        private PlayerControls _playerControls;

        public Trigger(string outOfRangeMessage, string addToBombMessage, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = GetName();
            _outOfRangeMessage = outOfRangeMessage;
            _addToBombMessage = addToBombMessage;
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite) 
            => new Trigger(_outOfRangeMessage, _addToBombMessage, slotIndex, quantity, maxQuantity, sprite);

        public override String GetName() => TriggerItem.Name;

        public override void SpawnDrop() => ItemSpawner.Instance.DropItem(ItemSpawner.Instance.triggerPickupPrefab, Quantity);

        public override void OnEquip() {}

        public override void OnUnEquip() {}

        public override void Update() => _execute |= _playerControls.Default.Shoot.WasPressedThisFrame();

        public override void FixedUpdate()
        {
            //******** Try to Execute if key pressed *******//
            if (!_execute) return;
            _execute = false;

            if (BombScript.Instance)
            {
                float distance = (BombScript.Instance.transform.position - GameManager.PlayerController.transform.position).magnitude;
                if (distance < 15)
                {
                    BombScript.Instance.AddTrigger();
                    DialogueManager.ShowNotification(_addToBombMessage);
                    Quantity = 0;
                }
                else
                {
                    DialogueManager.ShowNotification(_outOfRangeMessage);
                }
            }
        }
    }
}
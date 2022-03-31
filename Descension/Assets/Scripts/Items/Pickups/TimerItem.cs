using System;
using Environment;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Items.Pickups
{
    public class TimerItem : EquippableItem
    {
        public static string Name = "Timer";

        [Header("Timer")] 
        public float range = 10; // how close to bomb to add to bomb
        public string outOfRangeMessage = "Need to add this to the bomb.";
        public string addToBombMessage = "Timer added to bomb!";
        
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) 
            => new Timer(range, outOfRangeMessage, addToBombMessage, slotIndex, quantity, maxQuantity, inventorySprite);
    }
    
    
    // logic for explosives
    [Serializable]
    class Timer : Equippable
    {
        private float _range;
        private string _outOfRangeMessage;
        private string _addToBombMessage;
        private bool _execute;
        private PlayerControls _playerControls;

        public Timer(float range, string outOfRangeMessage, string addToBombMessage, int slotIndex, int quantity, int maxQuantity, Sprite sprite) : base(slotIndex, quantity, maxQuantity, sprite)
        {
            name = GetName();
            _range = range;
            _outOfRangeMessage = outOfRangeMessage;
            _addToBombMessage = addToBombMessage;
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
        
        public override Equippable DeepCopy(int slotIndex, int quantity, int maxQuantity, Sprite sprite) 
            => new Timer(_range, _outOfRangeMessage, _addToBombMessage, slotIndex, quantity, maxQuantity, sprite);

        public override String GetName() => TimerItem.Name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.TimerPrefab, GameManager.PlayerController.transform.position, Quantity);

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
                Debug.Log("Distance: " + distance);
                if (distance <= _range)
                {
                    BombScript.Instance.AddTimer();
                    DialogueManager.ShowNotification(_addToBombMessage);
                    Quantity = -1;
                }
                else
                {
                    DialogueManager.ShowNotification(_outOfRangeMessage);
                }
            }
        }
    }
}
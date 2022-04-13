using System;
using Actor.Player;
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
        public override Equippable CreateInstance(int slotIndex, int quantity) => new Timer(this, slotIndex, quantity);
    }
    
    
    // logic for timer
    [Serializable]
    internal class Timer : Equippable
    {
        // attributes
        private float _range;
        private string _outOfRangeMessage;
        private string _addToBombMessage;
        
        public Timer(TimerItem timerItem, int slotIndex, int quantity) : base(timerItem, slotIndex, quantity) 
            => Init(timerItem.range, timerItem.outOfRangeMessage, timerItem.addToBombMessage);
        
        public Timer(Timer timer) : base(timer) 
            => Init(timer._range, timer._outOfRangeMessage, timer._addToBombMessage);
        
        public void Init(float range, string outOfRangeMessage, string addToBombMessage)
        {
            name = TimerItem.Name;
            _range = range;
            _outOfRangeMessage = outOfRangeMessage;
            _addToBombMessage = addToBombMessage;
        }
        
        public override Equippable DeepCopy() => new Timer(this);

        public override string GetName() => TimerItem.Name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.TimerPrefab, PlayerPosition, Quantity);
        
        protected override void Execute()
        {
            if (BombScript.Instance)
            {
                var distance = (BombScript.Instance.transform.position - PlayerController.Position).magnitude;
                Debug.Log("Distance: " + distance);
                if (distance <= _range)
                {
                    BombScript.Instance.AddTimer();
                    DialogueManager.ShowPrompt(_addToBombMessage);
                    Quantity = -1;
                }
                else
                {
                    DialogueManager.ShowPrompt(_outOfRangeMessage);
                }
            }
        }
    }
}
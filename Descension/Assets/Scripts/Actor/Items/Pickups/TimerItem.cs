using System;
using Actor.Environment;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Helpers;

namespace Actor.Items.Pickups
{
    public class TimerItem : EquippableItem
    {
        public static string Name = "Timer";

        [Header("Timer")] 
        public float range = 10; // how close to bomb to add to bomb
        public string outOfRangeMessage = "Need to add this to the bomb.";
        public string addToBombMessage = "Timer added to bomb!";
        public float addToBombPromptTime = 1.5f;
        
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
        private float _addToBombPromptTime;
        
        public Timer(TimerItem timerItem, int slotIndex, int quantity) : base(timerItem, slotIndex, quantity) 
            => Init(timerItem.range, timerItem.outOfRangeMessage, timerItem.addToBombMessage, timerItem.addToBombPromptTime);
        
        public Timer(Timer timer) : base(timer) 
            => Init(timer._range, timer._outOfRangeMessage, timer._addToBombMessage, timer._addToBombPromptTime);
        
        public void Init(float range, string outOfRangeMessage, string addToBombMessage, float addToBombPromptTime)
        {
            name = TimerItem.Name;
            _range = range;
            _outOfRangeMessage = outOfRangeMessage;
            _addToBombMessage = addToBombMessage;
            _addToBombPromptTime = addToBombPromptTime;
        }
        
        public override Equippable DeepCopy() => new Timer(this);

        public override string GetName() => TimerItem.Name;

        public override void SpawnDrop() => SpawnManager.SpawnItem(SpawnManager.TimerPrefab, PlayerPosition, Quantity);
        
        public override void Execute()
        {
            base.Execute();

            if (Bomb.Instance)
            {
                var distance = (Bomb.Instance.transform.position - PlayerController.Position).magnitude;
                GameDebug.Log("Distance: " + distance);
                if (distance <= _range)
                {
                    Bomb.Instance.AddTimer();
                    DialogueManager.ShowPrompt(_addToBombMessage, _addToBombPromptTime);
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
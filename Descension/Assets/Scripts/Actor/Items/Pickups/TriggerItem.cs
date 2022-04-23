using System;
using Actor.Environment;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Helpers;

namespace Actor.Items.Pickups
{
    public class TriggerItem : EquippableItem
    {
        public static string Name = "Trigger";

        [Header("Trigger")] 
        public float range = 10; // how close to bomb to add to bomb
        public string outOfRangeMessage = "Need to add this to the bomb.";
        public string addToBombMessage = "Trigger added to bomb!";
        public float addToBombPromptTime = 1.5f;

        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) => new Trigger(this, slotIndex, quantity);
    }
    
    
    // logic for explosives
    [Serializable]
    internal class Trigger : Equippable
    {
        // attributes
        private float _range;
        private string _outOfRangeMessage;
        private string _addToBombMessage;
        private float _addToBombPromptTime;

        public Trigger(TriggerItem triggerItem, int slotIndex, int quantity) : base(triggerItem, slotIndex, quantity) 
            => Init(triggerItem.range, triggerItem.outOfRangeMessage, triggerItem.addToBombMessage, triggerItem.addToBombPromptTime);
        
        public Trigger(Trigger trigger) : base(trigger) 
            => Init(trigger._range, trigger._outOfRangeMessage, trigger._addToBombMessage, trigger._addToBombPromptTime);
        
        public void Init(float range, string outOfRangeMessage, string addToBombMessage, float addToBombPromptTime)
        {
            name = TriggerItem.Name;
            _range = range;
            _outOfRangeMessage = outOfRangeMessage;
            _addToBombMessage = addToBombMessage;
            _addToBombPromptTime = addToBombPromptTime;
        }
        
        public override Equippable DeepCopy() => new Trigger(this);

        public override string GetName() => name;

        public override void SpawnDrop() => SpawnManager.SpawnItem(SpawnManager.TriggerPrefab, PlayerPosition, Quantity);
        
        public override void Execute()
        {
            base.Execute();

            if (Bomb.Instance)
            {
                var distance = (Bomb.Instance.transform.position - PlayerController.Position).magnitude;
                GameDebug.Log("Distance: " + distance);
                if (distance <= _range)
                {
                    Bomb.Instance.AddTrigger();
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
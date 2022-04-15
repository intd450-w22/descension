using System;
using Actor.Player;
using Environment;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;
using Util.Helpers;

namespace Items.Pickups
{
    public class TriggerItem : EquippableItem
    {
        public static string Name = "Trigger";

        [Header("Trigger")] 
        public float range = 10; // how close to bomb to add to bomb
        public string outOfRangeMessage = "Need to add this to the bomb.";
        public string addToBombMessage = "Trigger added to bomb!";
        
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

        public Trigger(TriggerItem triggerItem, int slotIndex, int quantity) : base(triggerItem, slotIndex, quantity) 
            => Init(triggerItem.range, triggerItem.outOfRangeMessage, triggerItem.addToBombMessage);
        
        public Trigger(Trigger trigger) : base(trigger) 
            => Init(trigger._range, trigger._outOfRangeMessage, trigger._addToBombMessage);
        
        public void Init(float range, string outOfRangeMessage, string addToBombMessage)
        {
            name = TriggerItem.Name;
            _range = range;
            _outOfRangeMessage = outOfRangeMessage;
            _addToBombMessage = addToBombMessage;
        }
        
        public override Equippable DeepCopy() => new Trigger(this);

        public override string GetName() => name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.TriggerPrefab, PlayerPosition, Quantity);
        
        public override void Execute()
        {
            base.Execute();

            if (BombScript.Instance)
            {
                var distance = (BombScript.Instance.transform.position - PlayerController.Position).magnitude;
                GameDebug.Log("Distance: " + distance);
                if (distance <= _range)
                {
                    BombScript.Instance.AddTrigger();
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
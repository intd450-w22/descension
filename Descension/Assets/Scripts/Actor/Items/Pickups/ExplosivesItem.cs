using System;
using Actor.Environment;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Helpers;

namespace Actor.Items.Pickups
{
    public class ExplosivesItem : EquippableItem
    {
        public static string Name = "Explosives";

        [Header("Explosives")] 
        public float range = 10; // how close to bomb to add to bomb
        public string outOfRangeMessage = "Need to add this to the bomb.";
        public string addToBombMessage = "Explosives added to bomb!";
        public float addToBombPromptTime = 1.5f;
        
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity) => new Explosives(this, slotIndex, quantity);
    }
    
    
    // logic for explosives
    [Serializable]
    internal class Explosives : Equippable
    {
        // attributes
        private float _range;
        private string _outOfRangeMessage;
        private string _addToBombMessage;
        private float _addToBombPromptTime;
        
        public Explosives(ExplosivesItem explosivesItem, int slotIndex, int quantity) : base(explosivesItem, slotIndex, quantity) 
            => Init(explosivesItem.range, explosivesItem.outOfRangeMessage, explosivesItem.addToBombMessage, explosivesItem.addToBombPromptTime);
        
        public Explosives(Explosives explosives) : base(explosives) 
            => Init(explosives._range, explosives._outOfRangeMessage, explosives._addToBombMessage, explosives._addToBombPromptTime);
        
        public void Init(float range, string outOfRangeMessage, string addToBombMessage, float addToBombPromptTime)
        {
            name = ExplosivesItem.Name;
            _range = range;
            _outOfRangeMessage = outOfRangeMessage;
            _addToBombMessage = addToBombMessage;
            _addToBombPromptTime = addToBombPromptTime;
        }
        
        public override Equippable DeepCopy() => new Explosives(this);

        public override string GetName() => name;

        public override void SpawnDrop() => SpawnManager.SpawnItem(SpawnManager.ExplosivesPrefab, PlayerPosition, Quantity);


        public override void Execute()
        {
            base.Execute();

            if (Bomb.Instance)
            {
                var distance = (Bomb.Instance.transform.position - PlayerController.Position).magnitude;
                GameDebug.Log("Distance: " + distance);
                if (distance <= _range)
                {
                    GameDebug.Log("In Range");
                    Bomb.Instance.AddExplosives();
                    DialogueManager.ShowPrompt(_addToBombMessage, _addToBombPromptTime);
                    Quantity = -1;
                }
                else
                {
                    GameDebug.Log("Out of Range");

                    DialogueManager.ShowPrompt(_outOfRangeMessage);
                }
            }
        }
    }
}

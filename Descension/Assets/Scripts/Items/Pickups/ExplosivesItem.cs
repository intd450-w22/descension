using System;
using Actor.Player;
using Environment;
using Managers;
using UnityEngine;

namespace Items.Pickups
{
    public class ExplosivesItem : EquippableItem
    {
        public static string Name = "Explosives";

        [Header("Explosives")] 
        public float range = 10; // how close to bomb to add to bomb
        public string outOfRangeMessage = "Need to add this to the bomb.";
        public string addToBombMessage = "Explosives added to bomb!";
        
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
        
        public Explosives(ExplosivesItem explosivesItem, int slotIndex, int quantity) : base(explosivesItem, slotIndex, quantity) 
            => Init(explosivesItem.range, explosivesItem.outOfRangeMessage, explosivesItem.addToBombMessage);
        
        public Explosives(Explosives explosives) : base(explosives) 
            => Init(explosives._range, explosives._outOfRangeMessage, explosives._addToBombMessage);
        
        public void Init(float range, string outOfRangeMessage, string addToBombMessage)
        {
            name = ExplosivesItem.Name;
            _range = range;
            _outOfRangeMessage = outOfRangeMessage;
            _addToBombMessage = addToBombMessage;
        }
        
        public override Equippable DeepCopy() => new Explosives(this);

        public override string GetName() => name;

        public override void SpawnDrop() => ItemSpawner.SpawnItem(ItemSpawner.ExplosivesPrefab, PlayerPosition, Quantity);


        protected override void Execute()
        {
            if (BombScript.Instance)
            {
                var distance = (BombScript.Instance.transform.position - PlayerController.Position).magnitude;
                Debug.Log("Distance: " + distance);
                if (distance <= _range)
                {
                    Debug.Log("In Range");
                    BombScript.Instance.AddExplosives();
                    DialogueManager.ShowPrompt(_addToBombMessage);
                    Quantity = -1;
                }
                else
                {
                    Debug.Log("Out of Range");

                    DialogueManager.ShowPrompt(_outOfRangeMessage);
                }
            }
        }
    }
}

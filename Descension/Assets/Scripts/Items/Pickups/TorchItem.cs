using System;
using UnityEngine;

namespace Items.Pickups
{
    public class TorchItem : EquippableItem
    {
        public static string Name = "Torch";
        
        public override string GetName() => Name;

        // override just creates class instance, passes in editor set values
        public override Equippable CreateInstance(int slotIndex, int quantity)
        {
            Debug.LogWarning("Cannot create instance of Torch, not a slottable item");
            return null;
        }
    }
}

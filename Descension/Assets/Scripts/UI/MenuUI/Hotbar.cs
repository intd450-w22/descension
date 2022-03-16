using System.Collections.Generic;
using System.Linq;
using Items.Pickups;
using UI.MenuUI;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private List<HotbarSlot> _hotbarSlots;

    void Awake()
    {
        _hotbarSlots = GetComponentsInChildren<HotbarSlot>().ToList();
    }

    void Start()
    {
        // Deactivate all slots
        SetActive(-1);
    }

    public void SetActive(int slot)
    {
        for (var i = 0; i < _hotbarSlots.Count; i++)
        {
            if (i == slot)
                _hotbarSlots[i].Activate();
            else
                _hotbarSlots[i].Deactivate();
        }
    }

    public void PickupItem(EquippableItem item, int slot)
    {
        // Set sprite and quantity
        _hotbarSlots[slot].ItemSprite.sprite = item.inventorySprite;
        // _hotbarSlots[slot].Quantity = item.Durability; TODO Get this
    }

    public void DropItem(int slot)
    {
        // Clear sprite and quantity
        _hotbarSlots[slot].ItemSprite.sprite = null;
    }

}

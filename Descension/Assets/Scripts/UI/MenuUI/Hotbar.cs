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
        DeactivateAll();
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

    public void DeactivateAll() => _hotbarSlots.ForEach(x => x.Deactivate());

    public void PickupItem(Equippable item, int slot)
    {
        _hotbarSlots[slot].SetSprite(item.inventorySprite);
        _hotbarSlots[slot].SetQuantity(item.durability);
    }

    public void DropItem(int slot)
    {
        _hotbarSlots[slot].ClearSprite();
        _hotbarSlots[slot].ClearQuantity();
        _hotbarSlots[slot].Deactivate();
    }

}
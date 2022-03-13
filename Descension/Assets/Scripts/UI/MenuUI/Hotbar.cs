using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items.Pickups;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private List<Image> hotbarSlots;

    void Awake()
    {
        // TODO: Get Inventory capacity from somewhere
        SetReferences();
    }

    void Start()
    {
        
    }

    public void SetReferences()
    {
        hotbarSlots = GetComponentsInChildren<Image>().ToList();
    }

    public void SetActive(int slot)
    {

    }

    public void PickupItem(EquippableItem item, int slot)
    {
        // Set sprite and quantity
        hotbarSlots[slot].sprite = item.inventorySprite;
    }

    public void DropItem(int slot)
    {
        // Clear sprite and quantity
        hotbarSlots[slot].sprite = null;
    }

}

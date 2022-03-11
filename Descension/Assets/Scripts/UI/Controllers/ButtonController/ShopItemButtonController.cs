using System;
using Environment;
using Items;
using Items.Pickups;
using Managers;
using TMPro;
using UnityEngine;
using Util.Helpers;

namespace UI.Controllers.ButtonController
{
    public class ShopItemButtonController : ButtonController
    {
        public EquippableItem item;
        public int cost;
        
        private TMP_Text _itemText;
        private TMP_Text ItemText
        {
            get
            {
                if (_itemText == null) _itemText = GetComponent<TMP_Text>();
                return _itemText;
            }
        }

        private void OnValidate()
        {
            ItemText.text = item.GetName() + " (" + cost + " gold)";
        }

        protected override void OnButtonClicked()
        {
            Debug.Log(item.GetName() + " clicked! Costs " + cost);
            float gold = InventoryManager.Instance.gold;

            if (gold < cost)
            {
                // TODO Sound Effect - Error
                UIManager.Instance.GetShopUIController().DisplayFeedback("Not enough gold!");
                return;
            }

            if (!InventoryManager.Instance.PickupItem(item, 1))
            {
                // TODO Sound Effect - Error
                UIManager.Instance.GetShopUIController().DisplayFeedback("No room in inventory!");
                return;
            }
            
            InventoryManager.Instance.gold -= cost;
            SoundManager.Instance.ItemFound();
            UIManager.Instance.GetShopUIController().UpdateGold();
            UIManager.Instance.GetShopUIController().DisplayFeedback(item.GetName() + " purchased for " + cost + " gold!");
        }
    }
}

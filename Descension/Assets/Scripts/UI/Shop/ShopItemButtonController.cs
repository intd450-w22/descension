using Managers;
using TMPro;
using UI.Widgets;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ShopItemButtonController : MenuItem
    {
        public MultiButton[] buttons;

        public ShopItem shopItem;
        public TMP_Text nameText;
        public TMP_Text quantityText;
        public TMP_Text costText;
        public Image image;
        
        protected virtual void Start()
        {
            for (var i = 0; i < buttons.Length; ++i)
            {
                buttons[i].onClick.AddListener(OnButtonClicked);
            }
        }

        public void Set(ShopItem item)
        {
            shopItem = item;
            nameText.text = item.shopName.Length > 0 ? item.shopName : shopItem.item.GetName();
            quantityText.text = $"x{shopItem.quantity.ToString(),2}";
            costText.text = shopItem.cost.ToString();
        }
        
        protected virtual void OnButtonClicked()
        {
            float gold = InventoryManager.Gold;

            if (gold < shopItem.cost)
            {
                UIManager.GetShopUIController().DisplayFeedback("Not enough gold!");
                SoundManager.Error();
                return;
            }

            int quantity = shopItem.quantity;
            if (!InventoryManager.PickupItem(shopItem.item, ref quantity))
            {
                UIManager.GetShopUIController().DisplayFeedback("No room in inventory!");
                SoundManager.Error();
                return;
            }
            
            InventoryManager.Gold -= shopItem.cost;
            SoundManager.ItemFound();
            UIManager.GetShopUIController().UpdateGold();
            UIManager.GetShopUIController().DisplayFeedback(shopItem.item.GetName() + " purchased for " + shopItem.cost + " gold!");
        }
    }
}

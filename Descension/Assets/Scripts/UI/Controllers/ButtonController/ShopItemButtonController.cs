using Items.Pickups;
using Managers;
using TMPro;
using UI.Controllers.ShopUI;

namespace UI.Controllers.ButtonController
{
    public class ShopItemButtonController : ButtonController
    {
        public ShopItem shopItem;
        private TMP_Text _itemText;
        private TMP_Text ItemText
        {
            get
            {
                if (_itemText == null) _itemText = GetComponent<TMP_Text>();
                return _itemText;
            }
        }

        public void Set(ShopItem item)
        {
            shopItem = item;
            ItemText.text = shopItem.item.GetName() + " (" + shopItem.cost + " gold)";
        }
        
        protected override void OnButtonClicked()
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

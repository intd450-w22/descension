using Items.Pickups;
using Managers;
using TMPro;

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

        public void Set(EquippableItem item, int cost)
        {
            this.item = item;
            this.cost = cost;
            ItemText.text = item.GetName() + " (" + cost + " gold)";
        }
        
        protected override void OnButtonClicked()
        {
            float gold = InventoryManager.Instance.gold;

            if (gold < cost)
            {
                UIManager.Instance.GetShopUIController().DisplayFeedback("Not enough gold!");
                SoundManager.Instance.Error();
                return;
            }

            if (!InventoryManager.Instance.PickupItem(item, 1))
            {
                UIManager.Instance.GetShopUIController().DisplayFeedback("No room in inventory!");
                SoundManager.Instance.Error();
                return;
            }
            
            InventoryManager.Instance.gold -= cost;
            SoundManager.Instance.ItemFound();
            UIManager.Instance.GetShopUIController().UpdateGold();
            UIManager.Instance.GetShopUIController().DisplayFeedback(item.GetName() + " purchased for " + cost + " gold!");
        }
    }
}

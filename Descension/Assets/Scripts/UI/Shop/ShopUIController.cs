using System;
using System.Collections.Generic;
using Actor.Items.Pickups;
using Managers;
using TMPro;
using UnityEngine;
using Util.Helpers;

namespace UI.Shop
{
    public class ShopUIController : MonoBehaviour
    {
        public String goldText = "Gold: ";
        public GameObject shopMenuItemPrefab;
        public List<ShopItem> shopItems;  // Items set here will be spawned as menu items automatically on start

        [HideInInspector] public List<GameObject> spawned;
        private TMP_Text _goldText;
        private TMP_Text _feedbackText;

        private Transform _menuRoot;
        public Transform MenuRoot
        {
            get
            {
                if (_menuRoot == null) _menuRoot = gameObject.GetChildObject("ShopItems").transform;
                return _menuRoot;
            }
        }
        
        void Awake()
        {
            _goldText = gameObject.GetChildObject("ShopItems").GetChildObject("GoldText").GetComponent<TMP_Text>();
            _feedbackText = gameObject.GetChildObject("ShopItems").GetChildObject("FeedbackText").GetComponent<TMP_Text>();
        }

        void Start()
        {
            foreach (ShopItem shopItem in shopItems)
            {
                if (shopItem.item != null)
                {
                    GameObject menuItem = Instantiate(shopMenuItemPrefab, MenuRoot);
                    menuItem.GetComponent<ShopItemButtonController>().Set(shopItem);
                    menuItem.name = shopItem.item.GetName() + "MenuItem";
                    menuItem.transform.SetAsLastSibling();
                }
            }
            gameObject.GetChildObject("ShopItems").GetChildObject("ReturnMenuItem").transform.SetAsLastSibling();
        }

        public void UpdateGold()
        {
            _goldText.text = goldText + " " + InventoryManager.Gold;
        }

        public void DisplayFeedback(String text)
        {
            _feedbackText.text = text;
            CancelInvoke(nameof(ClearFeedbackText));
            Invoke(nameof(ClearFeedbackText), 3);
        }

        private void ClearFeedbackText()
        {
            _feedbackText.text = "";
        }
    }
    
    
    [Serializable]
    public struct ShopItem
    {
        public EquippableItem item;
        public string shopName;
        public int cost;
        public int quantity;
    }
}

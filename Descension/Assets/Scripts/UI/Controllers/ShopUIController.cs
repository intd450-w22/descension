using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util.Helpers;

namespace UI.Controllers
{
    public class ShopUIController : MonoBehaviour
    {
        public String goldText = "Gold: ";
        
        private TMP_Text _goldText;
        private TMP_Text _feedbackText;

        void Awake()
        {
            _goldText = gameObject.GetChildObjectWithName("ShopItems").GetChildObjectWithName("GoldText").GetComponent<TMP_Text>();
            _feedbackText = gameObject.GetChildObjectWithName("ShopItems").GetChildObjectWithName("FeedbackText").GetComponent<TMP_Text>();
        }

        public void UpdateGold()
        {
            _goldText.text = goldText + " " + InventoryManager.Instance.gold;
        }

        public void DisplayFeedback(String text)
        {
            _feedbackText.text = text;
            Invoke(nameof(ClearFeedbackText), 4);
        }

        private void ClearFeedbackText()
        {
            _feedbackText.text = "";
        }
    }
}

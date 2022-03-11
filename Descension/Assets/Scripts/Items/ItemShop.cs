using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Util.Enums;
using Util.Helpers;

namespace Items
{
    public class ItemShop : MonoBehaviour
    {
        public String dialogue = "Press F to open shop";
        public KeyCode activationKey = KeyCode.F;
        private bool _inRange;

        // Update is called once per frame
        void Update()
        {
            if (_inRange && Input.GetKeyDown(activationKey))
            {
                UIManager.Instance.SwitchUi(UIType.Shop);
                UIManager.Instance.GetShopUIController().UpdateGold();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            UIManager.Instance.GetHudController().ShowText(dialogue);
            if (other.gameObject.CompareTag("Player")) _inRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            UIManager.Instance.GetHudController().HideDialogue();
            if (other.gameObject.CompareTag("Player")) _inRange = false;
        }
    }
}

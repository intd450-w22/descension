﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util.Helpers;

namespace UI.MenuUI
{
    public class HotbarSlot : MonoBehaviour
    {
        [SerializeField]
        private Sprite _defaultSprite;
        [SerializeField]
        private Color _activeOutlineColour = new Color(255, 155, 0, 128);
        [SerializeField]
        private Color _inactiveOutlineColour = new Color(255, 255, 255, 16);

        public int Quantity { get; set; } = -1;

        public Image ItemImage { get; set; }
        public Image OutlineImage { get; set; }
        public TextMeshProUGUI QuantityText { get; set; }

        void Awake()
        {
            ItemImage = gameObject.GetChildObjectWithName("SlotItem").GetComponent<Image>();
            OutlineImage = gameObject.GetChildObjectWithName("SlotOutline").GetComponent<Image>();
            QuantityText = gameObject.GetChildObjectWithName("SlotQuantity").GetComponent<TextMeshProUGUI>();

            UpdateQuantity();
        }

        public void Activate()
        {
            OutlineImage.color = _activeOutlineColour;
        }

        public void Deactivate()
        {
            OutlineImage.color = _inactiveOutlineColour;
        }

        public void SetSprite(Sprite sprite)
        {
            ItemImage.sprite = sprite;
        }

        public void ClearSprite()
        {
            ItemImage.sprite = _defaultSprite;
        }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
            UpdateQuantity();
        }

        public void ClearQuantity()
        {
            Quantity = -1;
            UpdateQuantity();
        }

        private void UpdateQuantity() => QuantityText.text = Quantity < 0 ? string.Empty : Quantity.ToString();
    }
}
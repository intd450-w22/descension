using System;
using System.Collections.Generic;
using System.Linq;
using UI.Controllers.ButtonController;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.MenuUI
{
    [Serializable]
    public class MultiButton : Button
    {
        public ShopItemButtonController buttonController;

        public new ButtonClickedEvent onClick = new ButtonClickedEvent();

        protected override void Start()
        {
            base.Start();
            buttonController = GetComponentInParent<ShopItemButtonController>();
        }

        private void BaseOnPointerUp(PointerEventData eventData) => base.OnPointerUp(eventData);
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            foreach (var button in buttonController.buttons)
            {
                if (button != this) button.BaseOnPointerUp(eventData);
            }
        }

        private void BaseOnPointerEnter(PointerEventData eventData)
        {
            buttonController.image.enabled = true;
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            foreach (var button in buttonController.buttons)
            {
                if (button != this) button.BaseOnPointerEnter(eventData);
            }
        }

        private void BaseOnPointerExit(PointerEventData eventData)
        {
            buttonController.image.enabled = false;
            base.OnPointerExit(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            foreach (var button in buttonController.buttons)
            {
                if (button != this) button.BaseOnPointerExit(eventData);
            }
        }

        private void BaseOnPointerClick(PointerEventData eventData) => base.OnPointerClick(eventData);
        public override void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke();
            base.OnPointerClick(eventData);
            foreach (var button in buttonController.buttons)
            {
                if (button != this) button.BaseOnPointerClick(eventData);
            }
        }

        public override void OnSelect(BaseEventData eventData) {}

        private void BaseOnPointerDown(PointerEventData eventData) => base.OnPointerDown(eventData);
        public override void OnPointerDown(PointerEventData eventData)
        {
            
            base.OnPointerDown(eventData);
            foreach (var button in buttonController.buttons)
            {
                if (button != this) button.BaseOnPointerDown(eventData);
            }
        }
    }
}

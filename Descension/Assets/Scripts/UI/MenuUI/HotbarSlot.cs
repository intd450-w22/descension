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

        public Image ItemImage { get; set; }
        public Image OutlineImage { get; set; }
        public int Quantity { get; set; } = -1;

        void Awake()
        {
            ItemImage = gameObject.GetChildObjectWithName("SlotItem").GetComponent<Image>();
            OutlineImage = gameObject.GetChildObjectWithName("SlotOutline").GetComponent<Image>();
        }

        public void Activate()
        {
            OutlineImage.color = _activeOutlineColour;
        }

        public void Deactivate()
        {
            OutlineImage.color = _inactiveOutlineColour;
        }

        public void ClearSprite()
        {
            ItemImage.sprite = _defaultSprite;
        }
    }
}
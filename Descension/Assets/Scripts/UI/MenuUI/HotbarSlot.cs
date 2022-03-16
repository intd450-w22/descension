using UnityEngine;
using UnityEngine.UI;
using Util.Helpers;

namespace UI.MenuUI
{
    public class HotbarSlot : MonoBehaviour
    {
        [SerializeField]
        private Color ActiveColour;
        [SerializeField]
        private Color InactiveColour;

        public Image ItemSprite { get; set; }
        public Image OutlineSprite { get; set; }
        public int Quantity { get; set; } = -1;

        void Awake()
        {
            ItemSprite = gameObject.GetChildObjectWithName("SlotItem").GetComponent<Image>();
            OutlineSprite = gameObject.GetChildObjectWithName("SlotOutline").GetComponent<Image>();
        }

        public void Activate()
        {
            OutlineSprite.color = ActiveColour;
        }

        public void Deactivate()
        {
            OutlineSprite.color = InactiveColour;
        }
    }
}
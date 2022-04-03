using Managers;
using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class UINavButtonController : ButtonController
    {
        public UIType TargetUI;
        public bool clearInventoryCache;
        protected override void OnButtonClicked()
        {
            UIManager.SwitchUi(TargetUI);
            if (clearInventoryCache)
            {
                InventoryManager.ClearSlots();
                InventoryManager.ClearCachedSlots();
            }
        }
    }
}
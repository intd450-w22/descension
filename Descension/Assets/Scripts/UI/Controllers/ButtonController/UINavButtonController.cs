using Managers;
using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class UINavButtonController : ButtonController
    {
        public UIType TargetUI;
        public bool clearInventoryCache;
        public bool resetDestroyedCache;  // reset destroyed objects cache
        protected override void OnButtonClicked()
        {
            UIManager.SwitchUi(TargetUI);
            if (clearInventoryCache)
            {
                InventoryManager.ClearSlots();
                InventoryManager.ClearCachedSlots();
            }

            if (resetDestroyedCache) GameManager.ClearDestroyedCache();
        }
    }
}
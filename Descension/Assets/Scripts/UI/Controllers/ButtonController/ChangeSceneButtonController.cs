using Managers;
using UnityEditor;
using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class ChangeSceneButtonController : ButtonController
    {
        public SceneAsset scene;
        public UIType UiType = UIType.None;
        public bool clearInventoryCache;
        public bool resetDestroyedCache;  // reset destroyed objects cache
        
        protected override void OnButtonClicked()
        {
            GameManager.SwitchScene(scene, UiType);
            if (clearInventoryCache)
            {
                InventoryManager.ClearSlots();
                InventoryManager.ClearCachedSlots();
            }
            
            if (resetDestroyedCache) GameManager.ClearDestroyedCache();
        }
    }
}

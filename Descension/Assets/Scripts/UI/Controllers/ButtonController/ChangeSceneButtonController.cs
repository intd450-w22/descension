using Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.EditorHelpers;
using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class ChangeSceneButtonController : ButtonController
    {
        #if UNITY_EDITOR
        public SceneAsset scene;
        private void OnValidate() { if (scene != null) _scene = scene.name; }
        #endif
        [SerializeField, ReadOnly] private string _scene;
        
        public UIType UiType = UIType.None;
        public bool clearInventoryCache;
        public bool resetDestroyedCache;  // reset destroyed objects cache
        
        
        protected override void OnButtonClicked()
        {
            GameManager.SwitchScene(_scene, UiType);
            if (clearInventoryCache)
            {
                InventoryManager.ClearSlots();
                InventoryManager.ClearCachedSlots();
            }
            
            if (resetDestroyedCache) GameManager.ClearDestroyedCache();
        }
    }
}

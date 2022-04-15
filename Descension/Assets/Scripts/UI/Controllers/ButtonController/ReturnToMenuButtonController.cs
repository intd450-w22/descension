using Actor.Player;
using Managers;
using UnityEditor;
using UnityEngine;
using Util.EditorHelpers;
using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class ReturnToMenuButtonController : ButtonController
    {
        #if UNITY_EDITOR
        public SceneAsset scene;
        private void OnValidate() { if (scene != null) _scene = scene.name; }
        #endif  

        [SerializeField, ReadOnly] private string _scene;

        protected override void OnButtonClicked()
        {
            GameManager.SwitchScene(_scene, UIType.MainMenu);
            
            InventoryManager.ClearSlots();
            InventoryManager.ClearCachedSlots();
            GameManager.ClearDestroyedCache();

            PlayerController.Disable();
            PlayerController.Reset();
        }
    }
}

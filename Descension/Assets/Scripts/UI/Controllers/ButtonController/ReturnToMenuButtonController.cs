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
            PlayerController.Disable();
            
            GameManager.ClearGameCache();
            
            GameManager.SwitchScene(_scene, UIType.MainMenu);
        }
    }
}

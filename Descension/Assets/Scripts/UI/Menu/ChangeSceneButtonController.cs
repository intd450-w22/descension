using Managers;
using UI.Widgets;
using UnityEditor;
using UnityEngine;
using Util.EditorHelpers;
using Util.Enums;

namespace UI.Menu
{
    public class ChangeSceneButtonController : ButtonController
    {
        #if UNITY_EDITOR
        public SceneAsset scene;
        private void OnValidate() { if (scene != null) _scene = scene.name; }
        #endif
        [SerializeField, ReadOnly] private string _scene;
        
        public UIType UiType = UIType.None;

        protected override void OnButtonClicked() => GameManager.SwitchScene(_scene, UiType);
    }
}

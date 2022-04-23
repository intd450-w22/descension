using System;
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

        protected override void OnButtonClicked() => GameManager.SwitchScene(_scene, UiType);
    }
}

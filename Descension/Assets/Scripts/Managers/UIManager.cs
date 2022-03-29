using System.Collections.Generic;
using System.Linq;
using UI.Controllers;
using UI.Controllers.Codex;
using UI.Controllers.ShopUI;
using UI.Controllers.UIController;
using Util;
using Util.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = Util.Enums.Scene;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        private static UIManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<UIManager>();
                
                return _instance;
            }
            set => _instance = value;
        }

        private List<UIController> _uiControllers;
        private HUDController _hudController;
        private UIController _lastActiveUi;
        private UIController _previousActiveUI;
        private ShopUIController _shopUIController;
        private CodexController _codexController;

        public UIType DefaultUi = UIType.MainMenu;

        // Helper properties
        public static Hotbar Hotbar => Instance._hudController.Hotbar;

        protected void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                _instance = this;
            }

            _hudController = GetComponentInChildren<HUDController>();
            _shopUIController = GetComponentInChildren<ShopUIController>();
            _codexController = GetComponentInChildren<CodexController>();

            _uiControllers = FindObjectsOfType<UIController>().ToList();
            _uiControllers.ForEach(x => x.gameObject.SetActive(false));
            
            SwitchUi(DefaultUi);
        }
        
        public static HUDController GetHudController() => Instance._hudController;

        public static ShopUIController GetShopUIController() => Instance._shopUIController;

        public static CodexController GetCodexController() => Instance._codexController;

        public static string GetCurrentScene() => SceneManager.GetActiveScene().name;

        public static UIType GetPreviousUI() => Instance._previousActiveUI.uiType;

        public static void ReinitHudController() => Instance._ReinitHudController();
        private void _ReinitHudController()
        {
            _hudController = GetComponentInChildren<HUDController>();
            _hudController.Init();
        }

        public static void SwitchUi(UIType uiType) => Instance._SwitchUi(uiType);
        private void _SwitchUi(UIType uiType)
        {
            Debug.Log("Switch UI to " + uiType);
            if (_lastActiveUi != null) 
                _lastActiveUi.gameObject.SetActive(false);

            if(uiType == UIType.None) return;

            var targetUi = _uiControllers.FirstOrDefault(x => x.uiType == uiType);
            if (targetUi != null)
            {
                targetUi.gameObject.SetActive(true);
                targetUi.OnStart();
                _previousActiveUI = _lastActiveUi;
                _lastActiveUi = targetUi;
            }
            else
            {
                Debug.LogWarning($"Cannot find UI element with UI type '{uiType}'");
            }
        }

        public static void SwitchScene(Scene scene, UIType uiType = UIType.None) => Instance._SwitchScene(scene.ToString(), uiType);
        public static void SwitchScene(string scene, UIType uiType = UIType.None) => Instance._SwitchScene(scene, uiType);
        private void _SwitchScene(string scene, UIType uiType = UIType.None)
        {
            SceneLoader.Load(scene);
            SwitchUi(uiType);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UI.Controllers;
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
        public static UIManager Instance
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

        public UIType DefaultUi = UIType.MainMenu;

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
            _uiControllers = FindObjectsOfType<UIController>().ToList();
            _uiControllers.ForEach(x => x.gameObject.SetActive(false));
            SwitchUi(DefaultUi);
        }

        public HUDController GetHudController() => _hudController;

        public string GetCurrentScene() => SceneManager.GetActiveScene().name;

        public void ReinitHudController()
        {
            _hudController = GetComponentInChildren<HUDController>();
            _hudController.Init();
        }

        public void SwitchUi(UIType uiType)
        {
            if (_lastActiveUi != null)
                _lastActiveUi.gameObject.SetActive(false);

            if(uiType == UIType.None) return;

            var targetUi = _uiControllers.FirstOrDefault(x => x.uiType == uiType);
            if (targetUi != null)
            {
                targetUi.gameObject.SetActive(true);
                targetUi.OnStart();
                _lastActiveUi = targetUi;
            }
            else
            {
                Debug.LogWarning($"Cannot find UI element with UI type '{uiType}'");
            }
        }

        public void SwitchScene(Scene scene, UIType uiType = UIType.None) => SwitchScene(scene.ToString(), uiType);

        public void SwitchScene(string scene, UIType uiType = UIType.None)
        {
            SceneLoader.Load(scene);
            SwitchUi(uiType);
        }
    }
}

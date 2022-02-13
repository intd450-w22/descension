using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util.Enums;

namespace Assets.Scripts.GUI.MenuUI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        private List<UIController> _uiControllers;
        private UIController _lastActiveUi;

        public static UIManager GetInstance() => _instance;

        protected void Awake()
        {
            if (_instance == null)
            {
                DontDestroyOnLoad(gameObject);
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _uiControllers = GetComponentsInChildren<UIController>().ToList();
            _uiControllers.ForEach(x => x.gameObject.SetActive(false));
            SwitchUI(UIType.MainMenu);
        }

        public void SwitchUI(UIType uiType)
        {
            if (_lastActiveUi != null)
                _lastActiveUi.gameObject.SetActive(false);

            var targetUI = _uiControllers.FirstOrDefault(x => x.UiType == uiType);
            if (targetUI != null)
            {
                targetUI.gameObject.SetActive(true);
                _lastActiveUi = targetUI;
            }
        }

        public void SwitchScene(Scene scene)
        {
            if (_lastActiveUi != null)
                _lastActiveUi.gameObject.SetActive(false);

            SceneLoader.Load(scene);
        }
    }
}

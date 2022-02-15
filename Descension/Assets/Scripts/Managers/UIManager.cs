using System.Collections.Generic;
using System.Linq;
using UI.MenuUI;
using Util;
using Util.Enums;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        private List<UIController> uiControllers;
        private UIController LastActiveUI;

        public UIType defaultUI = UIType.MainMenu;

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
            
            uiControllers = FindObjectsOfType<UIController>().ToList();
            uiControllers.ForEach(x => x.gameObject.SetActive(false));
            SwitchUI(defaultUI);
        }

        public void SwitchUI(UIType uiType)
        {
            Debug.Log("SWITCH UI " + uiType);
            if (LastActiveUI != null)
                LastActiveUI.gameObject.SetActive(false);

            var targetUI = uiControllers.FirstOrDefault(x => x.uiType == uiType);
            if (targetUI != null)
            {
                targetUI.gameObject.SetActive(true);
                LastActiveUI = targetUI;
            }
            else
            {
                Debug.LogWarning($"Cannot find UI element with UI type '{uiType}'");
            }
        }

        public void SwitchScene(Scene scene)
        {
            if (LastActiveUI != null)
                LastActiveUI.gameObject.SetActive(false);

            SceneLoader.Load(scene);
        }

        public void SwitchScene(string scene)
        {
            if (LastActiveUI != null)
                LastActiveUI.gameObject.SetActive(false);

            SceneLoader.Load(scene);
        }
    }
}

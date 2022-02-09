using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util.Enums;

namespace Assets.Scripts.GUI.MenuUI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        private List<UIController> uiControllers;
        private UIController LastActiveUI;

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

            uiControllers = GetComponentsInChildren<UIController>().ToList();
            uiControllers.ForEach(x => x.gameObject.SetActive(false));
            SwitchUI(UIType.MainMenu);
        }

        public void SwitchUI(UIType uiType)
        {
            if (LastActiveUI != null)
                LastActiveUI.gameObject.SetActive(false);

            var targetUI = uiControllers.FirstOrDefault(x => x.uiType == uiType);
            if (targetUI != null)
            {
                targetUI.gameObject.SetActive(true);
                LastActiveUI = targetUI;
            }
        }

        public void SwitchScene(Scene scene)
        {
            if (LastActiveUI != null)
                LastActiveUI.gameObject.SetActive(false);

            SceneLoader.Load(scene);
        }
    }
}

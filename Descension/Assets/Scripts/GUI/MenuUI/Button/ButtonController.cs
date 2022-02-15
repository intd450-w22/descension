using UnityEngine;

namespace Assets.Scripts.GUI.MenuUI.Button
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class ButtonController : MenuItem
    {
        protected UIManager uiManager;
        protected UnityEngine.UI.Button button;

        protected virtual void Start()
        {
            button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(OnButtonClicked);
            uiManager = UIManager.GetInstance();
        }

        protected virtual void OnButtonClicked()
        {
            // override this 
        }

    
    }
}

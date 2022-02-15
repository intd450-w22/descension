using Managers;
using UI.MenuUI;
using UnityEngine;

namespace UI.Controllers.ButtonController
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
            uiManager = UIManager.Instance;
        }

        protected virtual void OnButtonClicked()
        {
            // override this 
        }

    
    }
}

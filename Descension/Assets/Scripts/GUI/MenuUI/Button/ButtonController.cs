using UnityEngine;

namespace Assets.Scripts.GUI.MenuUI.Button
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class ButtonController : MonoBehaviour
    {
        protected UIManager UiManager;
        protected UnityEngine.UI.Button Button;

        protected virtual void Start()
        {
            Button = GetComponent<UnityEngine.UI.Button>();
            Button.onClick.AddListener(OnButtonClicked);
            UiManager = UIManager.GetInstance();
        }

        protected virtual void OnButtonClicked()
        {
            // override this 
        }

    
    }
}

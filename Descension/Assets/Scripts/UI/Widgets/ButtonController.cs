using UnityEngine;

namespace UI.Widgets
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class ButtonController : MenuItem
    {
        protected UnityEngine.UI.Button button;

        protected virtual void Start()
        {
            button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(OnButtonClicked);
        }

        protected virtual void OnButtonClicked()
        {
            // override this 
        }
    }
}

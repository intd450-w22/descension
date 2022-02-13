using Util.Enums;

namespace Assets.Scripts.GUI.MenuUI.Button
{
    public class UINavButtonController : ButtonController
    {
        public UIType TargetUI;

        protected override void OnButtonClicked()
        {
            uiManager.SwitchUI(TargetUI);
        }
    }
}
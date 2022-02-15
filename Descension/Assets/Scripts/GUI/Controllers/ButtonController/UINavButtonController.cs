using Assets.Scripts.Util.Enums;

namespace Assets.Scripts.GUI.Controllers.ButtonController
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
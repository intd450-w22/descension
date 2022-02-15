using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class UINavButtonController : ButtonController
    {
        public UIType TargetUI;

        protected override void OnButtonClicked()
        {
            uiManager.SwitchUi(TargetUI);
        }
    }
}
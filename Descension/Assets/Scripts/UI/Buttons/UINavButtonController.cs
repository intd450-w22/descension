using Managers;
using UI.Widgets;
using Util.Enums;

namespace UI.Buttons
{
    public class UINavButtonController : ButtonController
    {
        public UIType TargetUI;

        protected override void OnButtonClicked() => UIManager.SwitchUi(TargetUI);
    }
}
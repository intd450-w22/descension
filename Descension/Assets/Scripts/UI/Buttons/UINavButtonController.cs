using System;
using Managers;
using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class UINavButtonController : ButtonController
    {
        public UIType TargetUI;

        protected override void OnButtonClicked() => UIManager.SwitchUi(TargetUI);
    }
}
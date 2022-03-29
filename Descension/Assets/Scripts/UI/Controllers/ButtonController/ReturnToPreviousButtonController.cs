using Managers;
using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class ReturnToPreviousButtonController : ButtonController
    {
        protected override void OnButtonClicked()
        {
            UIManager.SwitchUi(UIManager.GetPreviousUI());
        }
    }
}
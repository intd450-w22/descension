using Managers;
using UI.Widgets;

namespace UI.Buttons
{
    public class ReturnToPreviousButtonController : ButtonController
    {
        protected override void OnButtonClicked()
        {
            UIManager.SwitchUi(UIManager.GetPreviousUI());
        }
    }
}
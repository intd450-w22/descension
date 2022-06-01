using Managers;
using UI.Widgets;
using Util.Enums;

namespace UI.Buttons
{
    public class ResumeGameButtonController : ButtonController
    {
        protected override void OnButtonClicked()
        {
            UIManager.SwitchUi(UIType.GameHUD);
            GameManager.Resume();
        }
    }
}

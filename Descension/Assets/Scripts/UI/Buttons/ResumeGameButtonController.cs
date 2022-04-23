using Managers;
using Util.Enums;

namespace UI.Controllers.ButtonController
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

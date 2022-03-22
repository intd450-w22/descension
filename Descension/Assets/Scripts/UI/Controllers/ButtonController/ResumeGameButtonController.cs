using Actor.Player;
using Managers;
using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class ResumeGameButtonController : ButtonController
    {
        protected override void OnButtonClicked()
        {
            UIManager.SwitchUi(UIType.GameHUD);
        
            var player = GameManager.PlayerController;
            if (player != null)
            {
                player.OnResume();
            }
        }
    }
}

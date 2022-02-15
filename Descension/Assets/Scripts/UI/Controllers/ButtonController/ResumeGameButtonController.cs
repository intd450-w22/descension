using Actor.Player;
using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class ResumeGameButtonController : ButtonController
    {
        protected override void OnButtonClicked()
        {
            uiManager.SwitchUi(UIType.GameHUD);
        
            var player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.OnResume();
            }
        }
    }
}

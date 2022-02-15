using Assets.Scripts.Actor.Player;
using Assets.Scripts.Util.Enums;

namespace Assets.Scripts.GUI.Controllers.ButtonController
{
    public class ResumeGameButtonController : ButtonController
    {
        protected override void OnButtonClicked()
        {
            uiManager.SwitchUI(UIType.GameHUD);
        
            var player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.OnResume();
            }
        }
    }
}

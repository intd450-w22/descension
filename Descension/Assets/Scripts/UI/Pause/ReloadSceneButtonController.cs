using Actor.Player;
using Managers;
using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class ReloadSceneButtonController : ButtonController
    {
        public UIType uiAfterReload;

        protected override void OnButtonClicked()
        {
            GameManager.OnReloadScene();

            if(uiAfterReload == UIType.GameHUD) UIManager.GetHudController().Reset();
            
            GameManager.SwitchScene(GameManager.GetCurrentScene(), uiAfterReload);
        }
    }
}

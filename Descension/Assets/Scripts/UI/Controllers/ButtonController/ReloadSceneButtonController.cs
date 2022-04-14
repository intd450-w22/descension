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
            InventoryManager.OnReloadScene();
            PlayerController.OnReloadScene();
            GameManager.OnReloadScene();

            var currScene = GameManager.GetCurrentScene();
            if(uiAfterReload == UIType.GameHUD) UIManager.GetHudController().Reset();
            
            GameManager.SwitchScene(currScene, uiAfterReload);
        }
    }
}

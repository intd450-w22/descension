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
            
            var currScene = UIManager.GetCurrentScene();
            UIManager.SwitchScene(currScene);

            if(uiAfterReload == UIType.GameHUD)
                UIManager.GetHudController().Reset();

            UIManager.SwitchUi(uiAfterReload);
        }
    }
}

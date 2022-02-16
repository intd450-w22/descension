using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class ReloadSceneButtonController : ButtonController
    {
        public UIType uiAfterReload;

        protected override void OnButtonClicked()
        {
            var currScene = uiManager.GetCurrentScene();
            uiManager.SwitchScene(currScene);

            if(uiAfterReload == UIType.GameHUD)
                uiManager.GetHudController().Reset();

            uiManager.SwitchUi(uiAfterReload);
        }
    }
}

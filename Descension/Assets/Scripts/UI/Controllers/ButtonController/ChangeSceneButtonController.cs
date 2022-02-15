

using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class ChangeSceneButtonController : ButtonController
    {
        public Scene TargetScene;
        public string OtherTargetScene;

        protected override void OnButtonClicked()
        {
            if(TargetScene == Scene.Other)
                uiManager.SwitchScene(OtherTargetScene);
            else
                uiManager.SwitchScene(TargetScene);
        }
    }
}

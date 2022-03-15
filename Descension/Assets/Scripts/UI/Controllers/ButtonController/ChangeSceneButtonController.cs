using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class ChangeSceneButtonController : ButtonController
    {
        public Scene TargetScene;
        public string OtherTargetScene;

        public UIType UiType = UIType.None;

        protected override void OnButtonClicked()
        {
            if(TargetScene == Scene.Other)
                uiManager.SwitchScene(OtherTargetScene, UiType);
            else
                uiManager.SwitchScene(TargetScene, UiType);
        }
    }
}

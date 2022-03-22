using Managers;
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
                UIManager.SwitchScene(OtherTargetScene, UiType);
            else
                UIManager.SwitchScene(TargetScene, UiType);
        }
    }
}

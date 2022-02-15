

using Util.Enums;

namespace Assets.Scripts.GUI.MenuUI.Button
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

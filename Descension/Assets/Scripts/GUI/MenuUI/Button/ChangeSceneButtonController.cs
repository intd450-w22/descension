

using Util.Enums;

namespace Assets.Scripts.GUI.MenuUI.Button
{
    public class ChangeSceneButtonController : ButtonController
    {
        public Scene TargetScene;

        protected override void OnButtonClicked()
        {
            UiManager.SwitchScene(TargetScene);
        }
    }
}

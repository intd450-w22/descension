using Managers;

namespace UI.Codex.ButtonController
{
    public class CodexTabButtonController : Widgets.ButtonController
    {
        public CodexPageType codexTab;
        public bool IsActive;

        protected override void OnButtonClicked()
        {
            // Get codex controller
            // Change page to codexTab
            var codexController = UIManager.GetCodexController();
            codexController.SetPage(codexTab);
        }
    }
}

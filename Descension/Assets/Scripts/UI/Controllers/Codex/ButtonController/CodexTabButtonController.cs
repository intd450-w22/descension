using Managers;

namespace UI.Controllers.Codex.ButtonController
{
    public class CodexTabButtonController : Controllers.ButtonController.ButtonController
    {
        public CodexPageType codexTab;
        public bool IsActive;

        protected override void OnButtonClicked()
        {
            // Get codex controller
            // Change page to codexTab
            var codexController = UIManager.Instance.GetCodexController();
            codexController.SetPage(codexTab);
        }
    }
}

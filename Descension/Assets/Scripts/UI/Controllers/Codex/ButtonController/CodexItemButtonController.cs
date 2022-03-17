using TMPro;
using Util.Helpers;

namespace UI.Controllers.Codex.ButtonController
{
    public class CodexItemButtonController : Controllers.ButtonController.ButtonController
    {
        private CodexPageController _pageController;
        private CodexPageItem _pageItem;

        private TextMeshProUGUI _itemNameText;

        void Awake()
        {
            _itemNameText = GetComponent<TextMeshProUGUI>();
        }

        public void Init(CodexPageController pageController, CodexPageItem pageItem)
        {
            _pageController = pageController;
            _pageItem = pageItem;

            _itemNameText.text = pageItem.ItemName;
        }

        protected override void OnButtonClicked()
        {
            _pageController.SetDetails(_pageItem);
        }
    }
}

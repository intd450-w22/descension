using TMPro;
using Util.Helpers;

namespace UI.Controllers.Codex.ButtonController
{
    public class CodexItemButtonController : Controllers.ButtonController.ButtonController
    {
        public bool Visible
        {
            get => gameObject.activeInHierarchy;
            set => gameObject.SetActive(value);
        }
        
        private CodexPageController _pageController;
        public CodexPageItem PageItem;

        private TextMeshProUGUI _itemNameText;

        void Awake()
        {
            _itemNameText = GetComponent<TextMeshProUGUI>();
        }

        public void Init(CodexPageController pageController, CodexPageItem pageItem)
        {
            _pageController = pageController;
            PageItem = pageItem;

            _itemNameText.text = pageItem.ItemName;
        }

        protected override void OnButtonClicked()
        {
            _pageController.SetDetails(PageItem);
        }
    }
}

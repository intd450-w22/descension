using TMPro;

namespace UI.Codex.ButtonController
{
    public class CodexItemButtonController : Widgets.ButtonController
    {
        public bool Active
        {
            get => gameObject.activeInHierarchy;
            set => gameObject.SetActive(value);
        }

        private bool _visible = true;
        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                Active = _visible;
            }
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

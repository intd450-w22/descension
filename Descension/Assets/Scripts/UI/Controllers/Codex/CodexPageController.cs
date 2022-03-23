using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.Controllers.Codex.ButtonController;
using UnityEngine;
using UnityEngine.UI;
using Util.Helpers;

namespace UI.Controllers.Codex
{
    public class CodexPageController : MonoBehaviour
    {
        [Header("Page Configuration")]
        public string PageTitle;
        public CodexPageType PageType;

        [Header("Prefabs")]
        public GameObject CodexMenuItemPrefab;

        private List<CodexItemButtonController> _buttonControllers = new List<CodexItemButtonController>();

        // Left Page
        private TextMeshProUGUI _pageTitleText;
        private GameObject _pageItemContainer;
        
        // Right Page
        private Image _pageDetailImage;
        private TextMeshProUGUI _pageDetailText;

        void Awake()
        {
            var leftPage = gameObject.GetChildObjectWithName("LeftPage");
            _pageTitleText = leftPage.GetChildObjectWithName("PageTitle").GetComponent<TextMeshProUGUI>();
            _pageItemContainer = leftPage.GetChildObjectWithName("PageItems");

            var rightPage = gameObject.GetChildObjectWithName("RightPage");
            _pageDetailImage = rightPage.GetChildObjectWithName("PageDetailImage").GetComponent<Image>();
            _pageDetailText = rightPage.GetChildObjectWithName("PageDetailText").GetComponent<TextMeshProUGUI>();
        }

        public void Init(CodexPage page)
        {
            if(CodexMenuItemPrefab == null)
                Debug.LogWarning("CodexPage: CodexMenuItemPrefab not set.");

            PageTitle = page.PageTitle;
            PageType = page.PageType;

            _pageTitleText.text = PageTitle;

            foreach (var pageItem in page.PageItems)
                CreatePageItem(pageItem);

            SetDetails(page.PageItems.First());
        }

        public void CreatePageItem(CodexPageItem pageItem)
        {
            var pageItemGameObject = Instantiate(CodexMenuItemPrefab, _pageItemContainer.transform);
            pageItemGameObject.name = pageItem.ItemName.RemoveWhitespace() + "MenuItem";
            pageItemGameObject.transform.SetAsLastSibling();

            var btnController = pageItemGameObject.GetComponent<CodexItemButtonController>();
            btnController.Init(this, pageItem);

            _buttonControllers.Add(btnController);
        }

        public void SetDetails(CodexPageItem pageItem)
        {
            _pageDetailImage.sprite = pageItem.ItemSprite;
            _pageDetailText.text = pageItem.ItemDescription;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
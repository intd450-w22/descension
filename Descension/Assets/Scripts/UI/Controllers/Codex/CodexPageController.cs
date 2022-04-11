using System.Collections.Generic;
using System.Linq;
using Managers;
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

        private Sprite _defaultSprite;

        void Awake()
        {
            var leftPage = gameObject.GetChildObjectWithName("LeftPage");
            _pageTitleText = leftPage.GetChildObjectWithName("PageTitle").GetComponent<TextMeshProUGUI>();
            _pageItemContainer = leftPage.GetChildObjectWithName("PageItems");

            var rightPage = gameObject.GetChildObjectWithName("RightPage");
            _pageDetailImage = rightPage.GetChildObjectWithName("PageDetailImage").GetComponent<Image>();
            _pageDetailText = rightPage
                ?.GetChildObjectWithName("Scroll View")
                ?.GetChildObjectWithName("Viewport")
                ?.GetChildObjectWithName("PageDetailText")
                ?.GetComponent<TextMeshProUGUI>();

            _defaultSprite = _pageDetailImage.sprite;
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

        public void OnStart()
        {
            CheckFacts();
            SetFirstDetail();
        }

        public void SetFirstDetail()
        {
            var firstVisible = _buttonControllers.FirstOrDefault(x => x.Visible);
            if (firstVisible != null)
            {
                SetDetails(firstVisible.PageItem);
                Debug.Log("Setting first detail");
            }
            else
            {
                ClearDetails();
                Debug.Log("Clearing first detail");
            }
        }

        public void SetDetails(CodexPageItem pageItem)
        {
            _pageDetailImage.sprite = pageItem.ItemSprite;
            _pageDetailText.text = pageItem.ItemDescription;
        }

        public void ClearDetails()
        {
            _pageDetailImage.sprite = _defaultSprite;
            _pageDetailText.text = string.Empty;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void CheckFacts()
        {
            foreach (var item in _buttonControllers.Where(x => x.PageItem.Rule.Any()))
                item.Visible = FactManager.Query(item.PageItem.Rule);
        }
    }
}
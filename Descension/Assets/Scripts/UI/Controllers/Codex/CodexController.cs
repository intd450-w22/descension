using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util.Helpers;

namespace UI.Controllers.Codex
{
    public class CodexController : UIController.UIController
    {
        [Header("Page Configuration")] 
        public List<CodexPage> CodexPages; // readonly

        [Header("Prefabs")]
        public GameObject CodexPagePrefab; 
        public GameObject CodexMenuItemPrefab;

        private GameObject _codexPagesContainer;
        private List<CodexPageController> _codexPageControllers = new List<CodexPageController>();

        private CodexPageType? _currPageType;

        void Awake()
        {
            if (CodexPagePrefab == null)
                Debug.LogWarning("Codex: CodexPagePrefab not set.");
            if(CodexMenuItemPrefab == null)
                Debug.LogWarning("Codex: CodexMenuItemPrefab not set.");
            if(CodexPages.IsNullOrEmpty())
                Debug.LogWarning("Codex: No pages found.");

            _codexPagesContainer = gameObject.GetChildObjectWithName("CodexPages");
            if (_codexPagesContainer == null || _codexPagesContainer.transform == null)
                Debug.LogWarning("Codex: Can't find 'CodexPages' game object.");

            foreach (var page in CodexPages)
                CreatePage(page);

            _codexPageControllers.ForEach(x => x.Deactivate());
            SetPage(CodexPages.First().PageType);
        }

        public override void OnStart()
        {
            foreach(var page in _codexPageControllers)
                page.OnStart();
        }

        public void CreatePage(CodexPage page)
        {
            // instantiate prefab as child of CodexPages
            var pageGameObject = Instantiate(CodexPagePrefab, _codexPagesContainer.transform);
            pageGameObject.name = page.PageType + "Page";
            pageGameObject.transform.SetAsLastSibling();

            // set values 
            var pageController = pageGameObject.GetComponent<CodexPageController>();
            pageController.CodexMenuItemPrefab = CodexMenuItemPrefab;
            pageController.Init(page);

            // add reference to _codexPageControllers
            _codexPageControllers.Add(pageController);
        }

        public void SetPage(CodexPageType pageType)
        {
            if (_currPageType != null)
                _codexPageControllers.SingleOrDefault(x => x.PageType == _currPageType)?.Deactivate();

            var page = _codexPageControllers.SingleOrDefault(x => x.PageType == pageType);
            if (page != null)
            {
                page.Activate();
                _currPageType = pageType;
            }
            else Debug.LogWarning($"No page in codex of type {pageType}");
        }
    }
}
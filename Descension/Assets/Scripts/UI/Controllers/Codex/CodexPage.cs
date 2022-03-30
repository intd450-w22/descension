using System;
using System.Collections.Generic;
using Rules;
using UnityEngine;

namespace UI.Controllers.Codex
{
    // Inspector compatible component for creating pickups
    [Serializable]
    public class CodexPage
    {
        public string PageTitle;
        public CodexPageType PageType;
        public List<CodexPageItem> PageItems;
    }

    [Serializable]
    public class CodexPageItem
    {
        public string ItemName;
        public string ItemDescription;
        public Sprite ItemSprite;
        public Rule Rule;
    }

}
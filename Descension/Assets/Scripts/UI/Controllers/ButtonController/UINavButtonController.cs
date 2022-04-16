using System;
using Managers;
using Util.Enums;

namespace UI.Controllers.ButtonController
{
    public class UINavButtonController : ButtonController
    {
        public UIType TargetUI;
        // public ClearCacheOptions clearCache = ClearCacheOptions.All;

        protected override void OnButtonClicked()
        {
            UIManager.SwitchUi(TargetUI);
            
            // switch (clearCache)
            // {
            //     case ClearCacheOptions.No:
            //         break;
            //     case ClearCacheOptions.CurrentLevel:
            //         GameManager.ClearLevelCache();
            //         break;
            //     case ClearCacheOptions.All:
            //         GameManager.ClearGameCache();
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
            
        }
    }
}
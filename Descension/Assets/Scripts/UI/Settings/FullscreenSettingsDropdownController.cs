using UnityEngine;
using UnityEngine.UI;
using Util.Helpers;

namespace UI.Controllers.ButtonController
{
    

    public class FullscreenSettingsDropdownController : MonoBehaviour
    {

        public void OnDropdownChanged(int dropdown)
        {
            var fullScreenMode = (FullScreenMode) dropdown;
            GameDebug.Log("Full Screen Option Changed: " + fullScreenMode);
            Screen.fullScreenMode = fullScreenMode;
        }
    }
}
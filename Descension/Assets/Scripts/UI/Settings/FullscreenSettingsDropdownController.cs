using UnityEngine;
using Util.Helpers;

namespace UI.Settings
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
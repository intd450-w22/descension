using UnityEngine;

namespace Assets.Scripts.GUI.MenuUI.Button
{
    public class QuitButtonController : ButtonController
    {
        protected override void OnButtonClicked()
        {
            Application.Quit();
        }
    }
}
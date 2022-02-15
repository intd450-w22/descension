using UnityEngine;

namespace Assets.Scripts.GUI.Controllers.ButtonController
{
    public class QuitButtonController : ButtonController
    {
        protected override void OnButtonClicked()
        {
            Application.Quit();
        }
    }
}
using UnityEngine;

namespace UI.Controllers.ButtonController
{
    public class QuitButtonController : ButtonController
    {
        protected override void OnButtonClicked()
        {
            Application.Quit();
        }
    }
}
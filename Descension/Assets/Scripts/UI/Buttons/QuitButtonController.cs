using UI.Widgets;
using UnityEngine;

namespace UI.Buttons
{
    public class QuitButtonController : ButtonController
    {
        protected override void OnButtonClicked()
        {
            Application.Quit();
        }
    }
}
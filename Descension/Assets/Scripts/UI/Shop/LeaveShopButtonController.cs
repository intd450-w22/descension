using System.Collections.Generic;
using Actor.Environment;
using Managers;
using UI.Widgets;
using Util.Enums;

namespace UI.Shop
{
    public class LeaveShopButtonController : ButtonController
    {
        protected override void OnButtonClicked()
        {
            UIManager.SwitchUi(UIType.GameHUD);
            GameManager.Resume();

            var dialogue = new List<string> { ItemShop.GetRandomCloseShopDialogue() };
            DialogueManager.StartDialogue(ItemShop.Name, dialogue);
        }
    }
}

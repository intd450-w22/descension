using System.Collections.Generic;
using Items;
using Managers;
using Util.Enums;

namespace UI.Controllers.ButtonController
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

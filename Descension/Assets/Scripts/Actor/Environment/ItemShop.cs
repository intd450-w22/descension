using System.Collections.Generic;
using Actor.Interface;
using Managers;
using UnityEngine;
using Util.Enums;

namespace Actor.Environment
{
    public class ItemShop : AInteractable
    {
        public static string Name => "ShopKeeper";

        private static readonly List<string> StandardDialogue = new List<string>
        {
            "The man is muttering to himself. You catch a little before he notices you."
        };

        private static readonly List<string> LoreDialogue = new List<string>
        {
            "Not many adventurers make it back alive you know. Best gear up when you're here. ",
            "Fight any big ones down there? I hope you killed 'em if you did. It's what they deserve.",
            "I once lived in a village nearby. But that's a lifetime ago.",
            "I hope my wife doesn't haunt me for what I'm doing. My little girl neither.",
            "You asked me to take you here. Remember that while you're down there, eh?",
            "I miss them. So, fucking much.",
            "The military brought a big ol' machine down there once. Never did tell us what they were planning on doing."
        };

        private static readonly List<string> OpenShopDialogue = new List<string>
        {
            "What're you looking to buy?",
            "Can I help you with something?",
            "Take your time. I'm not going anywhere. Not like you should either. "
        };

        private static readonly List<string> CloseShopDialogue = new List<string>
        {
            "Good luck with whatever it is you came here for.",
            "Be careful down there. ",
            "Do come back when you need something more.",
            "You're going back? Really? I'll say a prayer for you while you're gone. "
        };

        public static void OpenShop()
        {
            SoundManager.OpenShop();
            GameManager.Pause();
            UIManager.SwitchUi(UIType.Shop);
            UIManager.GetShopUIController().UpdateGold();
        }

        public override void Interact() {
            var dialogue = GetRandomDialogue();
            DialogueManager.StartDialogue(Name, dialogue);
        }
        public override Vector2 Location() => gameObject.transform.position;
        public override string GetPrompt() => "Press F to buy";

        public static List<string> GetRandomDialogue()
        {
            var dialogue = new List<string>();
            dialogue.AddRange(StandardDialogue);
            dialogue.Add(GetRandomLoreDialogue());
            dialogue.Add(GetRandomOpenShopDialogue());
            dialogue.Add(">OpenShop");

            return dialogue;
        }
        public static string GetRandomLoreDialogue() => LoreDialogue[UnityEngine.Random.Range(0, LoreDialogue.Count)];
        public static string GetRandomOpenShopDialogue() => OpenShopDialogue[UnityEngine.Random.Range(0, OpenShopDialogue.Count)];
        public static string GetRandomCloseShopDialogue() => CloseShopDialogue[UnityEngine.Random.Range(0, CloseShopDialogue.Count)];

    }
}

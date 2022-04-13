using System;
using Managers;
using UnityEngine;
using Util.Enums;

using System.Collections.Generic;

namespace Items
{
    public class ItemShop : MonoBehaviour
    {
        public KeyCode shopActivationKey = KeyCode.F; // TODO: Refactor to use the input system
        public KeyCode talkActivationKey = KeyCode.T; // TODO: Refactor to use the input system
        private bool _inRange;
        private bool _talked = false;
        private DialogueManager _dialogueManager;

        private string _name = "ShopKeeper";
        private List<string> _standardDialogue = new List<string>();
        private List<string> _loreDialogue = new List<string>();
        private List<string> _openShopDialogue = new List<string>();
        private List<string> _closeShopDialogue = new List<string>();

        void Awake()
        {
            InitDialogue();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.IsFrozen || !_inRange) return;

            if (Input.GetKeyDown(shopActivationKey)) 
            {
                GameManager.Pause();
                UIManager.SwitchUi(UIType.Shop);
                UIManager.GetShopUIController().UpdateGold();
            }
            else if (Input.GetKeyDown(talkActivationKey)) 
            {
                var dialogue = new List<string>();
                dialogue.AddRange(_standardDialogue);
                dialogue.Add(_loreDialogue[UnityEngine.Random.Range(0, _loreDialogue.Count)]);
                dialogue.Add(_openShopDialogue[UnityEngine.Random.Range(0, _openShopDialogue.Count)]);
                dialogue.Add("Press F to open the shop.");
                dialogue.Add(_closeShopDialogue[UnityEngine.Random.Range(0, _closeShopDialogue.Count)]);

                DialogueManager.StartDialogue(_name, dialogue);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("OnTriggerEnter2d");
                _inRange = true;
                DialogueManager.ShowPrompt("Press T to talk   Press F to buy");   
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _inRange = false;
                DialogueManager.HidePrompt();
            }
        }

        private void InitDialogue() {
            _standardDialogue.Add("The man is muttering to himself.");
            _standardDialogue.Add("You catch a little before he notices you.");

            _loreDialogue.Add("Not many adventurers make it back alive you know. Best gear up when you're here. ");
            _loreDialogue.Add("Fight any big ones down there? I hope you killed 'em if you did. It's what they deserve.");
            _loreDialogue.Add("I once lived in a village nearby. But that's a lifetime ago.");
            _loreDialogue.Add("I hope my wife doesn't haunt me for what I'm doing. My little girl neither.");
            _loreDialogue.Add("You asked me to take you here. Remember that while you're down there, eh?");
            _loreDialogue.Add("I miss them. So, fucking much.");
            _loreDialogue.Add("The military brought a big ol' machine down there once. Never did tell us what they were planning on doing.");

            _openShopDialogue.Add("What're you looking to buy?");
            _openShopDialogue.Add("Can I help you with something?");
            _openShopDialogue.Add("Take your time. I'm not going anywhere. Not like you should either. ");

            _closeShopDialogue.Add("Good luck with whatever it is you came here for.");
            _closeShopDialogue.Add("Be careful down there. ");
            _closeShopDialogue.Add("Do come back when you need something more.");
            _closeShopDialogue.Add("You're going back? Really? I'll say a prayer for you while you're gone. ");
        }
    }
}

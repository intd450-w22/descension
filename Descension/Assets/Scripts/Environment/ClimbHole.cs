using Actor.Player;
using Managers;
using UI.Controllers;
using Util;
using Util.Enums;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Environment
{
    public class ClimbHole : MonoBehaviour
    {
        public Scene nextLevel;
        public string otherLevelName;
        public string showText;
        public bool endGame;
        private Action _endGame;

        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.GetHudController();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                DialogueManager.ShowPrompt(showText);
                if (endGame)
                {
                    _endGame += EndGame;
                    DialogueManager.StartDialogue("ShopKeeper", new[] { "Thought you’d never make it. Thank you. You’ve done more for me than you think. But there are other places like this. Are you willing to do it again?" }, _endGame);
                }
                else
                {
                    if (nextLevel == Scene.Other)
                        SceneLoader.Load(otherLevelName);
                    else
                        SceneLoader.Load(nextLevel.ToString());
                }
            }
        }
        void EndGame()
        {
            UIManager.SwitchUi(UIType.End);
        }
    }
}
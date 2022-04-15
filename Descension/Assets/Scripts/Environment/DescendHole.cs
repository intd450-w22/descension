using Actor.Interface;
using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEditor;
using Util;
using Util.Enums;
using UnityEngine;
using System;

namespace Environment
{
    public class DescendHole : UniqueMonoBehaviour
    {
        public SceneAsset nextLevel;
        public int nextLevelStartPosition;
        public string showText;
        public bool needsRope = true;
        public bool leaveHole;
        private Action _endGame;

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player"))
            {
                // check if descended already
                needsRope = needsRope && !GameManager.IsUniqueDestroyed(this, out _);
                if (leaveHole)
                {
                    _endGame += EndGame;
                    DialogueManager.StartDialogue("ShopKeeper", new[] { "Thought you�d never make it. Thank you. You�ve done more for me than you think. But there are other places like this. Are you willing to do it again?" }, _endGame);
                }
                else
                {
                    if (!needsRope || PlayerController.Instance.ropeQuantity > 0)
                    {
                        if (needsRope) PlayerController.AddRope(-1);
                        if (showText.Length > 0) DialogueManager.ShowPrompt(showText);
                        GameManager.CacheDestroyedUnique(this, transform.position);
                        GameManager.OnSceneComplete();
                        GameManager.SwitchScene(nextLevel, UIType.None, nextLevelStartPosition);
                    }
                    else
                    {
                        DialogueManager.ShowPrompt("You need a rope in order to descend");
                    }
                }
            }
        }
   

        void EndGame()
        {
            UIManager.SwitchUi(UIType.End);
        }
    }
}

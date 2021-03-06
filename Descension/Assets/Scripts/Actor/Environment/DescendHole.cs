using System;
using Actor.Interface;
using Actor.Player;
using Managers;
using UnityEditor;
using UnityEngine;
using Util.EditorHelpers;
using Util.Enums;
using Util.Helpers;
using static Util.Helpers.CalculationHelper;

namespace Actor.Environment
{
    public class DescendHole : MonoBehaviour, IUnique
    {
        [SerializeField, ReadOnly] private int uniqueId;
        public int GetUniqueId() => uniqueId;
        public void SetUniqueId(int id) => uniqueId = id;
        
        #if UNITY_EDITOR
        public SceneAsset nextLevel;
        private void OnValidate() { if (nextLevel != null) _nextLevel = nextLevel.name; }
        #endif
        [SerializeField, ReadOnly] private string _nextLevel;
        
        public int nextLevelStartPosition;
        public string showText;
        public bool needsRope = true;
        public bool leaveHole;
        private Action _endGame;
        public FactKey Fact;

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player"))
            {
                // check if descended already
                needsRope = needsRope && !GameManager.IsUniqueDestroyed(this);
                if (leaveHole)
                {
                    if (FactManager.IsFactTrue(Fact))
                    {
                        _endGame += EndGame;
                        DialogueManager.StartDialogue("ShopKeeper", new[] { "Thought you'd never make it. Thank you. You've done more for me than you think. But there are other places like this. Are you willing to do it again?" }, _endGame);
                    }
                    else
                    {
                        DialogueManager.StartDialogue("ShopKeeper", new[] { "You just got here, you can't leave yet." });
                    }
                }
                else
                {
                    if (!needsRope || PlayerController.Instance.ropeQuantity > 0)
                    {
                        SoundManager.Descend();
                        if (needsRope) PlayerController.AddRope(-1);
                        if (showText.Length > 0) DialogueManager.ShowPrompt(showText);
                        GameManager.DestroyUnique(this);
                        GameManager.OnSceneComplete();
                        GameManager.SwitchScene(_nextLevel, UIType.None, nextLevelStartPosition);
                    }
                    else
                    {
                        DialogueManager.ShowPrompt("You need a rope in order to descend");
                        this.InvokeWhen(DialogueManager.HidePrompt, () => DistanceSq(PlayerController.Position, transform.position) > 200, 2);
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

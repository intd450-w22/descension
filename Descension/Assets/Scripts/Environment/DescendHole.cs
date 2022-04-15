using Actor.Interface;
using Actor.Player;
using Managers;
using UnityEditor;
using Util.Enums;
using UnityEngine;
using Util.EditorHelpers;

namespace Environment
{
    public class DescendHole : UniqueMonoBehaviour
    {
        #if UNITY_EDITOR
        public SceneAsset nextLevel;
        private void OnValidate() { if (nextLevel != null) _nextLevel = nextLevel.name; }
        #endif
        
        [SerializeField, ReadOnly] private string _nextLevel;
        
        public int nextLevelStartPosition;
        public string showText;
        public bool needsRope = true;


        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player"))
            {
                // check if descended already
                needsRope = needsRope && !IsUniqueDestroyed();
                
                if (!needsRope || PlayerController.Instance.ropeQuantity > 0) {
                    DestroyUnique();
                    if (needsRope) PlayerController.AddRope(-1);
                    if (showText.Length > 0) DialogueManager.ShowPrompt(showText);
                    GameManager.OnSceneComplete();
                    GameManager.SwitchScene(_nextLevel, UIType.None, nextLevelStartPosition);
                } else {
                    DialogueManager.ShowPrompt("You need a rope in order to descend");
                }
            }
        }
    }
}

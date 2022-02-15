using Actor.Player;
using UnityEngine;
using UnityEngine.UI;
using Util.Enums;

namespace Environment
{
    public class DescendHole : MonoBehaviour
    {
        public Scene nextLevel;
        public string otherLevelName;

        public Image dialogueBox;
        public Text dialogueText;
    
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                if (FindObjectOfType<PlayerController>().ropeQuantity > 0) {
                    FindObjectOfType<PlayerController>().AddRope(-1);
                    showText("Descend to level two...");

                    if(nextLevel == Scene.Other)
                        SceneLoader.Load(otherLevelName);
                    else
                        SceneLoader.Load(nextLevel.ToString());   
                    
                } else {
                    showText("You need a rope in order to descend");
                }
            }
        }
        void showText(string text) {
            dialogueBox.enabled = true;
            dialogueText.enabled = true;
            dialogueText.text = text;
        }
    }
}

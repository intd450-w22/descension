using Actor.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Environment
{
    public class LevelTwoHole : MonoBehaviour
    {
        public Image dialogueBox;
        public Text dialogueText;
    
        void OnCollisionEnter2D(Collision2D collision) {
            // Debug.Log("collided");
            if (collision.gameObject.name == "Player") {
                if (FindObjectOfType<PlayerController>().ropeQuantity > 0) {
                    FindObjectOfType<PlayerController>().addRope(-1);
                    showText("Descend to level two...");
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

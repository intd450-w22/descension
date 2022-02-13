using Actor.Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Environment
{
    public class DescendHole : MonoBehaviour
    {
        public int nextLevel;
        public Image dialogueBox;
        public Text dialogueText;
    
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.collider.tag == "Player") {
                if (FindObjectOfType<PlayerController>().ropeQuantity > 0) {
                    FindObjectOfType<PlayerController>().addRope(-1);
                    showText("Descend to level two...");
                    SceneManager.LoadScene(nextLevel);
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

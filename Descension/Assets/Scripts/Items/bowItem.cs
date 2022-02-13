using Actor.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class BowItem : MonoBehaviour
    {
        public Image dialogueBox;
        public Text dialogueText;
        
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.collider.tag == "Player") {
                FindObjectOfType<PlayerController>().addBow();
                showText("Bow collected");
                Destroy(gameObject);
            }
        }

        void showText(string text) {
            dialogueBox.enabled = true;
            dialogueText.enabled = true;
            dialogueText.text = text;
        }
    }
}

using Actor.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ArrowsItem : MonoBehaviour
    {
        public Image dialogueBox;
        public Text dialogueText;
        public float quantity = 20;

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.name == "Player") {
                FindObjectOfType<PlayerController>().AddArrows(this.quantity);
                showText("Arrows collected");
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
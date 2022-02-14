using Actor.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ropeItem : MonoBehaviour
    {
        public Image dialogueBox;
        public Text dialogueText;
        public float quantity = 1;
        
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<PlayerController>().AddRope(this.quantity);
                showText("Rope Collected");
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

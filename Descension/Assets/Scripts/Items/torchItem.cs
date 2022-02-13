using Actor.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class TorchItem : MonoBehaviour
    {
        public Image dialogueBox;
        public Text dialogueText;
        public float quantity = 20;
        
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.collider.tag == "Player") {
                FindObjectOfType<PlayerController>().addTorch(this.quantity);
                showText("Torch Collected");
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

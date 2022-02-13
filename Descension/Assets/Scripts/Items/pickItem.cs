using Actor.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class pickItem : MonoBehaviour
    {
        public Image dialogueBox;
        public Text dialogueText;
        public float quantity = 20;
        // public GameObject PlayerController;

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.collider.tag == "Player") {
                FindObjectOfType<PlayerController>().AddPick(this.quantity);
                showText("Pick Collected");
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

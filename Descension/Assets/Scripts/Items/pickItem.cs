using Actor.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class PickItem : MonoBehaviour
    {
        public Image dialogueBox;
        public Text dialogueText;
        public float quantity = 20;
        // public GameObject PlayerController;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
        
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.name == "Player") {
                FindObjectOfType<PlayerController>().addPick(this.quantity);
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

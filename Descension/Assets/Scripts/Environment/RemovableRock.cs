using Actor.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Environment
{
    public class RemovableRock : MonoBehaviour
    {
        public Image dialogueBox;
        public Text dialogueText;
        public GameObject floatingText;

        private float lootChance = 40;

        void Start() {
            dialogueBox.enabled = false;
            dialogueText.enabled = false;
        }

        void OnCollisionEnter2D(Collision2D collision) {
            // TODO: Find a better way to do this logic. Maybe use a "Player" Tag. 
            if (collision.gameObject.CompareTag("Player")) {
                if (FindObjectOfType<PlayerController>().pickQuantity > 0) {
                    if (Random.Range(0f, 100f) < this.lootChance) {
                        float gold = Mathf.Floor(Random.Range(0f, 20f));
                        FindObjectOfType<PlayerController>().score += gold;
                        showFloatingText("Gold +" + gold.ToString());
                    }
                
                    FindObjectOfType<PlayerController>().AddPick(-1);
                    Destroy(gameObject);
                } else {
                    showText("Find a pick!");
                }
            }
        }

        void showText(string text) {
            dialogueBox.enabled = true;
            dialogueText.enabled = true;
            dialogueText.text = text;
        }

        void showFloatingText(string text) {
            var t = Instantiate(floatingText, transform.position, Quaternion.identity);
            t.GetComponent<TextMesh>().text = text;
        }
    }
}

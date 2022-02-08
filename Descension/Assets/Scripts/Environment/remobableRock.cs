using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class remobableRock : MonoBehaviour
{
    public Image dialogueBox;
    public Text dialogueText;
    public GameObject floatingText;

    private float lootChance = 40;

    // Start is called before the first frame update
    void Start() {
        dialogueBox.enabled = false;
        dialogueText.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player") {
            // this works if there is only one "player" object
            if (FindObjectOfType<player>().pickQuantity > 0) {
                if (Random.Range(0f, 100f) < this.lootChance) {
                    float gold = Mathf.Floor(Random.Range(0f, 20f));
                    FindObjectOfType<player>().score += gold;
                    showFloatingText("Gold +" + gold.ToString());
                }
                
                FindObjectOfType<player>().addPick(-1);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arrowsItem : MonoBehaviour
{
    public Image dialogueBox;
    public Text dialogueText;
    public float quantity = 20;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player") {
            FindObjectOfType<player>().addArrows(this.quantity);
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
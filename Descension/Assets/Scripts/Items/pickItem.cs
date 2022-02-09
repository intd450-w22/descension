using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickItem : MonoBehaviour
{
    public Image dialogueBox;
    public Text dialogueText;
    public float quantity = 20;
    // public GameObject player;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player") {
            FindObjectOfType<player>().addPick(this.quantity);
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

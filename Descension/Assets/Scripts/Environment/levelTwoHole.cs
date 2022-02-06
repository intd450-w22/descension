using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelTwoHole : MonoBehaviour
{
    public Image dialogueBox;
    public Text dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("collided");
        if (collision.gameObject.name == "Player") {
            if (FindObjectOfType<player>().hasRope > 0) {
                FindObjectOfType<player>().hasRope -= 1;
                showText("Descend to level two...");
            } else {
                showText("You need a rope in order to descend");
            }

        }
    }

    void showText(string text) {
        dialogueBox.enabled = true;
        dialogueText.enabled = true;
        dialogueText.text = text;
    }
}

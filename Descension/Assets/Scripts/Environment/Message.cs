using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public string textToShow;
    public Image dialogueBox;
    public Text dialogueText;

    public bool triggered = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        triggered = true;
        showText(textToShow);
    }

    void showText(string text) {
        dialogueBox.enabled = true;
        dialogueText.enabled = true;
        dialogueText.text = text;
    }
}

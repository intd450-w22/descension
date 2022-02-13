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

    private void OnTriggerEnter2D(Collider2D other) {
        if (!triggered) {
            showText(textToShow);
            triggered = true;
        }
    }

    void showText(string text) {
        dialogueBox.enabled = true;
        dialogueText.enabled = true;
        dialogueText.text = text;
    }
}

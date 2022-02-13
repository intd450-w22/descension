using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectableObject : MonoBehaviour
{
    public string inspectText;
    public Image dialogueBox;
    public Text dialogueText;

    private bool playerInRange = false;

    // Update is called once per frame
    void Update() {
        if (playerInRange && Input.GetKeyDown(KeyCode.F)) {
            showText(inspectText);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        playerInRange = true;
        showText("Press F to interact");
    }

    private void OnTriggerExit2D(Collider2D other) {
        playerInRange = false;
    }

    void showText(string text) {
        dialogueBox.enabled = true;
        dialogueText.enabled = true;
        dialogueText.text = text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.Rendering.PostProcessing;

public class player : MonoBehaviour
{
    public float movementSpeed = 10;
    public float hasPick = 0;
    public float hasBow = 0;
    public float hasRope = 0;
    public float hasTorch = 0;

    public Image dialogueBox;
    public Text dialogueText;
    public Text scoreUI;
    public Text bowUI;
    public Text pickUI;
    public Text torchUI;
    public Text ropeUI;
    public GameObject floatingTextDamage;
    public GameObject arrow;

    public float hitPoints = 100f;
    public float score = 0f;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.enabled = false;
        dialogueText.enabled = false;
        scoreUI.enabled = true;
        bowUI.enabled = false;
        pickUI.enabled = false;
        torchUI.enabled = false;
        ropeUI.enabled = false;

        camera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        scoreUI.text = "Gold/Score: " + score.ToString();

        if (this.hasPick > 0) {
            pickUI.enabled = true;
            pickUI.text = "Pick " + this.hasPick.ToString();
        } else {
            pickUI.enabled = false;
        }

        if (this.hasBow > 0) {
            bowUI.enabled = true;
            bowUI.text = "Arrows " + this.hasBow.ToString();
        } else {
            bowUI.enabled = false;
        }

        if (this.hasRope > 0) {
            ropeUI.enabled = true;
            ropeUI.text = "Rope " + this.hasRope.ToString();
        } else {
            ropeUI.enabled = false;
        }

        if (this.hasTorch > 0) {
            torchUI.enabled = true;
            torchUI.text = "Torch " + Mathf.Floor(this.hasTorch).ToString();
        } else {
            torchUI.enabled = false;
        }

        if (this.hasTorch > 0) {
            this.hasTorch -= 2 * Time.deltaTime;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (x != 0 || y != 0) {
            transform.Translate(x * movementSpeed * Time.deltaTime,  y * movementSpeed * Time.deltaTime, 0);
        }

        if ((dialogueBox.enabled || dialogueText.enabled) && Input.GetKeyDown(KeyCode.Space)) {
            dialogueBox.enabled = false;
            dialogueText.enabled = false;
        }

        if (Input.GetMouseButtonDown(0) && hasBow > 0) {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 screenPoint = camera.WorldToScreenPoint(transform.localPosition);
            Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            Instantiate(arrow, transform.position, Quaternion.Euler(0f, 0f, angle));
            this.hasBow -= 1;
        }
    }

    public void addPick(float value) {
        this.hasPick += value;
    }

    public void addBow(float value) {
        this.hasBow += value;
    }

    public void addRope(float value) {
        this.hasRope += value;
    }

    public void addTorch(float value) {
        this.hasTorch += value;
    }

    public void inflictDamage(float damage) {
        this.hitPoints -= damage;
        showFloatingTextDamage("HP -" + damage.ToString());
    }

    void showFloatingTextDamage(string text) {
        var t = Instantiate(floatingTextDamage, transform.position, Quaternion.identity);
        t.GetComponent<TextMesh>().text = text;
    }
}

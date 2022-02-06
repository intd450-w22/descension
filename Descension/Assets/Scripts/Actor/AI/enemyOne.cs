using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyOne : MonoBehaviour
{
    public float hitPoints = 100f;
    public float damage = 15f;

    public GameObject floatingTextDialogue;
    public GameObject floatingTextDamage;

    // Vector3 movement = new Vector3(0.1f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.hitPoints < 0) {
            this.showFloatingTextDialogue("DEAD!");
            Destroy(gameObject);
        }

        // Debug.Log(transform.position.x);

        // transform.position += movement * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player") {
            FindObjectOfType<player>().inflictDamage(this.damage);
            // this.showFloatingTextDialogue("AAAW");
        }
    }

    public void inflictDamage(float damage) {
        this.hitPoints -= damage;
        showFloatingTextDamage("HP -" + damage.ToString());
    }

    void showFloatingTextDamage(string text) {
        var t = Instantiate(floatingTextDamage, transform.position, Quaternion.identity);
        t.GetComponent<TextMesh>().text = text;
    }

    void showFloatingTextDialogue(string text) {
        var t = Instantiate(floatingTextDialogue, transform.position, Quaternion.identity);
        t.GetComponent<TextMesh>().text = text;
    }
}

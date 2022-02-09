using System.Collections;
using System.Collections.Generic;
using Actor.AI;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float speed = 8;
    public float damage = 10;
    public Rigidbody2D body; 
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        body.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        // set "Enemy" Tag to enemy object for this to work
        if (collision.CompareTag("Enemy")) {
            // Debug.Log("attacked enemy");
            collision.gameObject.GetComponent<AIController>().InflictDamage(this.damage);
            Destroy(gameObject);
        }
    }
}

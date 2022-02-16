using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actor.Player;

public class AiCollision : MonoBehaviour { 
    public float damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collision enemy");
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Player"))
        {
            obj.GetComponent<PlayerController>().InflictDamage(damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actor.Player;

public class Transport : MonoBehaviour
{
    public Vector2 transportLocation;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            FindObjectOfType<PlayerController>().transform.position = transportLocation;
        }
    }
}

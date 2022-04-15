using UnityEngine;
using Actor.Player;
using Managers;

public class Transport : MonoBehaviour
{
    public Vector2 transportLocation;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            SoundManager.Descend();
            FindObjectOfType<PlayerController>().transform.position = transportLocation;
        }
    }
}

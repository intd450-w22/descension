using UnityEngine;
using Actor.Player;
using Managers;
using Util.Enums;
using System.Collections.Generic;

public class Transport : MonoBehaviour
{
    public Vector2 transportLocation;
    public bool isEscapeEnding;
    public FactKey Fact;

    void OnCollisionEnter2D(Collision2D collision) {
        if (isEscapeEnding)
        {
            if (FactManager.IsFactTrue(Fact))
            {
                DialogueManager.StartDialogue("", new List<string>
                {
                    "This path has mysteriously closed, the only way out is the way you came in"
                });
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                SoundManager.Descend();
                FindObjectOfType<PlayerController>().transform.position = transportLocation;
            }
        }
        else if (collision.gameObject.CompareTag("Player")) {
            SoundManager.Descend();
            FindObjectOfType<PlayerController>().transform.position = transportLocation;
        }
    }
}

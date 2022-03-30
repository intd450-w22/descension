using UnityEngine;
using Actor.Player;
using Managers;

public class AiCollision : MonoBehaviour { 
    public float damage = 10;
    public float knockBack = 0;
    // Start is called before the first frame update
   
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Player"))
        {
            GameManager.PlayerController.InflictDamage(gameObject, damage, knockBack);
        }
    }
}

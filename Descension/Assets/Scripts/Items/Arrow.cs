using Actor.AI;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Arrow : MonoBehaviour
    {
        // TODO: Given that this is an entity and not a consumable item,
        // TODO: this class should be moved elsewhere later.

        public float speed = 8;
        public float damage = 10;
        public float timeToLive = 3f;
        public Rigidbody2D body; 
        
        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            Destroy(gameObject, timeToLive);
        }

        // Update is called once per frame
        void Update() {
            body.velocity = transform.right * speed;
        }

        void OnTriggerEnter2D(Collider2D collision) {
            // set "Enemy" Tag to enemy object for this to work
            Debug.Log("Arrow collision : " + collision.tag);
            if (collision.CompareTag("Enemy"))
            {
                // Debug.Log("attacked enemy");
                try { collision.gameObject.GetComponent<AIController>().InflictDamage(this.damage); }
                catch { collision.gameObject.GetComponentInParent<AIController>().InflictDamage(this.damage); }
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Environment"))
                Destroy(gameObject);
        }
    }
}

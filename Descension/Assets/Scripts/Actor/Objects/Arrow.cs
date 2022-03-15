using Actor.AI;
using Managers;
using Util.Enums;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Arrow : MonoBehaviour
    {
        // TODO: Given that this is an entity and not a consumable item,
        // TODO: this class should be moved elsewhere later.

        public float speed = 20;
        public float damage = 10;
        public float timeToLive = 3f;
        public Rigidbody2D body;
        public Transform sprite;

        private Vector2 _velocity;
        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            Destroy(gameObject, timeToLive);
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.IsPaused) return;

            body.velocity = _velocity;
        }

        void OnTriggerEnter2D(Collider2D collision) {
            // set "Enemy" Tag to enemy object for this to work
            if (collision.CompareTag(Tag.Enemy.ToString()))
            {
                try { collision.gameObject.GetComponent<AIController>().InflictDamage(this.damage); }
                catch { collision.gameObject.GetComponentInParent<AIController>().InflictDamage(this.damage); }
                Destroy(gameObject);
            }
            else if (collision.CompareTag(Tag.Environment.ToString()))
                Destroy(gameObject);
        }

        public void Initialize(Vector2 direction)
        {
            direction.Normalize();
            float angle = Vector2.SignedAngle(direction, Vector2.up);
            sprite.Rotate(Vector3.forward, -angle);
            _velocity = direction * speed;
            
        }
    }
}

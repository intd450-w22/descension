using Actor.AI;
using Actor.Interface;
using Managers;
using UnityEngine;
using Util.Enums;

namespace Actor.Objects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Arrow : Projectile
    {
        public float speed = 20;
        public float timeToLive = 3f;
        public Rigidbody2D body;
        public Transform sprite;

        private float _damage;
        private Tag _targetTag;
        private Vector2 _velocity;
        
        public override void Initialize(Vector2 direction, float damage, Tag targetTag)
        {
            direction.Normalize();
            float angle = Vector2.SignedAngle(direction, Vector2.up);
            sprite.Rotate(Vector3.forward, -angle);
            _damage = damage;
            _velocity = direction * speed;
            _targetTag = targetTag;
        }

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            SoundManager.ArrowAttack();
            Destroy(gameObject, timeToLive);
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.IsFrozen) return;

            body.velocity = _velocity;
        }

        void OnTriggerEnter2D(Collider2D collision) {
            // set "Enemy" Tag to enemy object for this to work
            if (collision.CompareTag(_targetTag.ToString()))
            {
                try { collision.gameObject.GetComponent<IDamageable>().InflictDamage(gameObject, _damage); }
                catch { collision.gameObject.GetComponentInParent<IDamageable>().InflictDamage(gameObject, _damage); }
                Destroy(gameObject);
            }
            else if (collision.CompareTag(Tag.Environment.ToString()))
                Destroy(gameObject);
        }
    }
}

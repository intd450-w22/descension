using Actor.Interface;
using Managers;
using UnityEngine;
using Util.Enums;
using Util.Helpers;

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
        private float _knockBack;
        private Tag _targetTag;
        private Vector2 _velocity;
        
        protected override void Initialize(Vector2 direction, float damage, float knockBack, Tag targetTag)
        {
            direction.Normalize();
            float angle = Vector2.SignedAngle(direction, Vector2.up);
            sprite.Rotate(Vector3.forward, -angle);
            _damage = damage;
            _velocity = direction * speed;
            _knockBack = knockBack;
            _targetTag = targetTag;
        }

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            SoundManager.ArrowAttack();
            Invoke(nameof(_Destroy), timeToLive);
        }

        private void _Destroy() => Destroy(gameObject);

        // Update is called once per frame
        void Update()
        {
            if (GameManager.IsFrozen)
            {
                body.velocity = Vector2.zero;
                CancelInvoke(nameof(_Destroy));
                
                // reactivate destroy timer when unfrozen
                this.InvokeWhen(
                    () => Invoke(nameof(_Destroy), timeToLive), 
                    () => !GameManager.IsFrozen, 
                    1);
                
                return;
            }

            body.velocity = _velocity;
        }

        void OnTriggerEnter2D(Collider2D collision) {
            // set "Enemy" Tag to enemy object for this to work
            if (collision.CompareTag(_targetTag.ToString()))
            {
                collision.gameObject
                    .GetComponent<IDamageable>(true)
                    .InflictDamage(_damage, _velocity.normalized, _knockBack);
                
                Destroy(gameObject);
            }
            else if (collision.CompareTag(Tag.Environment.ToString()))
                Destroy(gameObject);
        }
    }
}

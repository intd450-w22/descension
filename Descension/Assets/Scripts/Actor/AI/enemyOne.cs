using Actor.Player;
using UnityEngine;

namespace Actor.AI
{
    public class EnemyOne : MonoBehaviour
    {
        public float hitPoints = 100f;
        public float damage = 15f;

        public GameObject floatingTextDialogue;
        public GameObject floatingTextDamage;

        Vector3 movement;

        float pointA = 66f;
        float pointB = 67f;

        float patrolTarget;

        void Start()
        {
            patrolTarget = pointB;
        }

        // Update is called once per frame
        void Update()
        {
            if (this.hitPoints < 0) {
                // this.showFloatingTextDialogue("DEAD!");
                Destroy(gameObject);
            }

            // Debug.Log(transform.position.x);

            if (patrolTarget == pointB) {
                movement = new Vector3(2f, 0f, 0f);
            } else if (patrolTarget == pointA) {
                movement = new Vector3(-2f, 0f, 0f);
            }

            transform.position += movement * Time.deltaTime;

            if (patrolTarget == pointB && transform.position.x >= pointB) {
                patrolTarget = pointA;
            } 
            if (patrolTarget == pointA && transform.position.x <= pointA) {
                patrolTarget = pointB;
            }
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.name == "Player") {
                FindObjectOfType<PlayerController>().InflictDamage(this.damage);
                // this.showFloatingTextDialogue("AAAW");
            }
        }

        public void inflictDamage(float damage) {
            this.hitPoints -= damage;
            showFloatingTextDamage("HP -" + damage.ToString());
        }

        void showFloatingTextDamage(string text) {
            var t = Instantiate(floatingTextDamage, transform.position, Quaternion.identity);
            t.GetComponent<TextMesh>().text = text;
        }

        void showFloatingTextDialogue(string text) {
            var t = Instantiate(floatingTextDialogue, transform.position, Quaternion.identity);
            t.GetComponent<TextMesh>().text = text;
        }
    }
}

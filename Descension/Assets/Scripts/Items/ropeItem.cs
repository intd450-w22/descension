using Actor.Player;
using UI.Controllers;
using UnityEngine;

namespace Items
{
    public class ropeItem : MonoBehaviour
    {
        public float quantity = 1;

        private HUDController _hudController;

        void Awake()
        {
            _hudController = FindObjectOfType<HUDController>();
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<PlayerController>().AddRope(this.quantity);
                _hudController.ShowText("Rope Collected");
                Destroy(gameObject);
            }
        }

    }
}

using Actor.Player;
using UI.Controllers;
using UnityEngine;

namespace Items
{
    public class arrowsItem : MonoBehaviour
    {
        public float quantity = 20;

        private HUDController _hudController;

        void Awake()
        {
            _hudController = FindObjectOfType<HUDController>();
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<PlayerController>().AddArrows(this.quantity);
                _hudController.ShowText("Arrows collected");
                Destroy(gameObject);
            }
        }

    }
}
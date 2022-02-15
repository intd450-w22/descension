using Actor.Player;
using Managers;
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
            _hudController = UIManager.Instance.GetHudController();
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<PlayerController>().AddRope(this.quantity);
                UIManager.Instance.GetHudController().ShowText("Rope Collected");
                Destroy(gameObject);
            }
        }

    }
}

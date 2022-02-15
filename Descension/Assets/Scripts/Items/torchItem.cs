using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEngine;

namespace Items
{
    public class torchItem : MonoBehaviour
    {
        public float quantity = 20;
        
        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.Instance.GetHudController();
        }
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<PlayerController>().AddTorch(this.quantity);
                UIManager.Instance.GetHudController().ShowText("Torch Collected");
                Destroy(gameObject);
            }
        }
    }
}

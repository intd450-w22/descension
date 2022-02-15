using Assets.Scripts.Actor.Player;
using Assets.Scripts.GUI.Controllers;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class torchItem : MonoBehaviour
    {
        public float quantity = 20;
        
        private HUDController _hudController;

        void Awake()
        {
            _hudController = FindObjectOfType<HUDController>();
        }
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<PlayerController>().AddTorch(this.quantity);
                _hudController.ShowText("Torch Collected");
                Destroy(gameObject);
            }
        }
    }
}

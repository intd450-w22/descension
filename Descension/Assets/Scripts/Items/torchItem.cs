using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEngine;
using Environment;

namespace Items
{
    public class torchItem : MonoBehaviour
    {
        public float quantity = 20;
        
        private string description = "Torch Collected.\n Use with caution. There are things down here that have more eyes than you.";
        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.Instance.GetHudController();
        }
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<SoundManager>().ItemFound();
                FindObjectOfType<PlayerController>().AddTorch(this.quantity);
                UIManager.Instance.GetHudController().ShowText(description);
                Destroy(gameObject);
            }
        }
    }
}

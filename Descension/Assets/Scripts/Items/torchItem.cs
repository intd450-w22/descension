using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEngine;

namespace Items
{
    public class torchItem : MonoBehaviour
    {
        public float quantity = 20;
        
        private bool _isPickedUp = false;
        private string _description = "Torch Collected.\n Use with caution. There are things down here that have more eyes than you.";
        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.Instance.GetHudController();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isPickedUp) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                _isPickedUp = true;
                FindObjectOfType<SoundManager>().ItemFound();
                FindObjectOfType<PlayerController>().AddTorch(quantity);
                UIManager.Instance.GetHudController().ShowText(_description);
                Destroy(gameObject);
            }
        }
    }
}

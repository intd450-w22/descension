using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEngine;
using Environment;

namespace Items
{
    public class arrowsItem : MonoBehaviour
    {
        public float quantity = 20;

        private bool _isPickedUp = false;
        private string _description = "Arrows collected.\n";
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
                FindObjectOfType<PlayerController>().AddArrows(quantity);
                UIManager.Instance.GetHudController().ShowText(_description);
                Destroy(gameObject);
            }
        }

    }
}
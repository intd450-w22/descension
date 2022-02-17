using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEngine;
using Environment;

namespace Items
{
    public class pickItem : MonoBehaviour
    {
        public float quantity = 20;

        private bool _isPickedUp = false;
        private string _description = "Pick Collected.\n Made for mining since the ancient eras. Who knew we'd still be using them?";
        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.Instance.GetHudController();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isPickedUp) return;
            
            FindObjectOfType<SoundManager>().ItemFound();
            if (collision.gameObject.CompareTag("Player"))
            {
                _isPickedUp = true;
                FindObjectOfType<PlayerController>().AddPick(quantity);
                UIManager.Instance.GetHudController().ShowText(_description);
                Destroy(gameObject);
            }
        }
    }
}

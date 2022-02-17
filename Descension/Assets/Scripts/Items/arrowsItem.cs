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

        private string description = "Arrows collected.\n";
        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.Instance.GetHudController();
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<SoundManager>().ItemFound();
                FindObjectOfType<PlayerController>().AddArrows(this.quantity);
                UIManager.Instance.GetHudController().ShowText(description);
                Destroy(gameObject);
            }
        }

    }
}
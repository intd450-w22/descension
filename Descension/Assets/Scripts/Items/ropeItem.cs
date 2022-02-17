using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEngine;
using Environment;

namespace Items
{
    public class ropeItem : MonoBehaviour
    {
        public float quantity = 1;

        private string description = "Rope Collected.\nNecessary for going further into the mines, or other things too.";
        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.Instance.GetHudController();
        }

        void OnCollisionEnter2D(Collision2D collision) {
            FindObjectOfType<SoundManager>().ItemFound();
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<PlayerController>().AddRope(this.quantity);
                UIManager.Instance.GetHudController().ShowText(description);
                Destroy(gameObject);
            }
        }

    }
}

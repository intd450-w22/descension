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

        private string description = "Pick Collected.\n Made for mining since the ancient eras. Who knew we'd still be using them?";
        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.Instance.GetHudController();
        }

        void OnCollisionEnter2D(Collision2D collision) {
            FindObjectOfType<SoundManager>().ItemFound();
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<PlayerController>().AddPick(this.quantity);
                UIManager.Instance.GetHudController().ShowText(description);
                Destroy(gameObject);
            }
        }
    }
}

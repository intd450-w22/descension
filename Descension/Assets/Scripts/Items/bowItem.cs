using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEngine;

namespace Items
{
    public class bowItem : MonoBehaviour
    {
        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.Instance.GetHudController();
        }
        
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<PlayerController>().AddBow();
                UIManager.Instance.GetHudController().ShowText("Bow collected");
                Destroy(gameObject);
            }
        }
    }
}

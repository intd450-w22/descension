using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEngine;
using Environment;

namespace Items
{
    public class bowItem : MonoBehaviour
    {
        private string description = "Bow collected.\nIt may just be wire on a stick, but it sure helps keep you safe down here.";
        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.Instance.GetHudController();
        }
        
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<SoundManager>().ItemFound();
                FindObjectOfType<PlayerController>().AddBow();
                UIManager.Instance.GetHudController().ShowText(description);
                Destroy(gameObject);
            }
        }
    }
}

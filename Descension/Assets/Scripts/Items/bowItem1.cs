using Actor.Player;
using Managers;
using UI.Controllers;
using UnityEngine;
using Environment;

namespace Items
{
    public class bowItem1 : MonoBehaviour
    {
        private bool _isPickedUp = false;
        private string _description = "Bow collected.\nIt may just be wire on a stick, but it sure helps keep you safe down here.";
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
                FindObjectOfType<PlayerController>().AddBow();
                UIManager.Instance.GetHudController().ShowText(_description);
                Destroy(gameObject);
            }
        }
    }
}

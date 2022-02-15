using Assets.Scripts.Actor.Player;
using Assets.Scripts.GUI.Controllers;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class bowItem : MonoBehaviour
    {
        private HUDController _hudController;

        void Awake()
        {
            _hudController = FindObjectOfType<HUDController>();
        }
        
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                FindObjectOfType<PlayerController>().AddBow();
                _hudController.ShowText("Bow collected");
                Destroy(gameObject);
            }
        }
    }
}

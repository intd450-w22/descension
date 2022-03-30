using Managers;
using UI.Controllers;
using Util.Enums;
using UnityEngine;

namespace Environment
{
    public class EndGame : MonoBehaviour
    {
        private HUDController _hudController;

        void Awake()
        {
            _hudController = UIManager.GetHudController();
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                UIManager.SwitchUi(UIType.End);
            }
        }
    }
}

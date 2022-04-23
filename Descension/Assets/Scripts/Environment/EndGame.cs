using Managers;
using UI.Controllers;
using Util.Enums;
using UnityEngine;

namespace Environment
{
    public class EndGame : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.CompareTag("Player")) {
                UIManager.SwitchUi(UIType.End);
            }
        }
    }
}

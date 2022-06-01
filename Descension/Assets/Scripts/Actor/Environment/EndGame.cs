using Managers;
using UnityEngine;
using Util.Enums;

namespace Actor.Environment
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

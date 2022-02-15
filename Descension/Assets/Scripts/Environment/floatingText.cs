using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class floatingText : MonoBehaviour
    {
        public float duration = 1f;

        void Start() {
            Destroy(gameObject, duration);
        }

    }
}

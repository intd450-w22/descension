using UnityEngine;

namespace Environment
{
    public class FloatingText : MonoBehaviour
    {
        public float duration = 1f;

        void Start() {
            Destroy(gameObject, duration);
        }

    }
}

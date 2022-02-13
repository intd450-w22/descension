using UnityEngine;

namespace Environment
{
    public class floatingText : MonoBehaviour
    {
        public float duration = 1f;

        void Start() {
            Destroy(gameObject, duration);
        }

    }
}

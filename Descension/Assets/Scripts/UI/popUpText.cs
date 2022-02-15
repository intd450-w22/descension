using UnityEngine;

namespace UI
{
    public class PopUpText : MonoBehaviour
    {
        public float duration = 3f;

        void Start()
        {
            Destroy(gameObject, duration);
        }

    }
}

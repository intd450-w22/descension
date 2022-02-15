using UnityEngine;

namespace Assets.Scripts.GUI
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

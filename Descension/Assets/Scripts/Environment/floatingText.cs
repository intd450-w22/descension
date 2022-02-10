using UnityEngine;

namespace Environment
{
    public class FloatingText : MonoBehaviour
    {
        public float duration = 1f;

        // Start is called before the first frame update
        void Start() {
            Destroy(gameObject, duration);
        }

        // Update is called once per frame
        void Update() {
        
        }
    }
}

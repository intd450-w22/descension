using Actor.Player;
using Managers;
using UnityEngine;

namespace Environment
{
    public class mainCameraFollow : MonoBehaviour {

        public Transform target;
        public float speed = 0.125f;

        // zoom out from target
        public Vector3 offset = new Vector3(0f, 0f, -1f);

        void Awake()
        {
            if (target == null) target = PlayerController.Instance.transform;
        }

        void LateUpdate() {
            transform.position = PlayerController.Instance.transform.position + offset;
        }
    }
}

using Actor.Player;
using Managers;
using UnityEngine;

namespace Environment
{
    public class mainCameraFollow : MonoBehaviour {
        public float speed = 0.125f;

        // zoom out from target
        public Vector3 offset = new Vector3(0f, 0f, -5f);
        
        void LateUpdate() => transform.position = PlayerController.Position + offset;
    }
}

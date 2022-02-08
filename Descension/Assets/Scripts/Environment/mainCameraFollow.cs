using UnityEngine;

public class mainCameraFollow : MonoBehaviour {
    public Transform target;
    public float speed = 0.125f;

    // zoom out from target
    public Vector3 offset = new Vector3(0f, 0f, -1f);

    void LateUpdate() {
        transform.position = target.position + offset;
    }
}

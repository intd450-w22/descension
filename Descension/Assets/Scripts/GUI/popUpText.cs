using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popUpText : MonoBehaviour
{
    public float duration = 3f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

}

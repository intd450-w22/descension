using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    public float duration = 3f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

}

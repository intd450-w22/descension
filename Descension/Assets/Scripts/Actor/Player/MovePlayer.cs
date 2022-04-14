using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Util.Enums;

public class MovePlayer : MonoBehaviour
{
    public FactKey Fact;
    public int x;
    public int y;
    // Start is called before the first frame update
    void Start()
    {
        if (FactManager.IsFactTrue(Fact))  {
            transform.position = new Vector3(x, y, 0);
        }   
    }
}

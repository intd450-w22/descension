using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class Util
    {
        static public readonly List<Vector2> Directions = new List<Vector2>()
        {
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(-1,0),
            new Vector2(0,-1),
        };
    
        public static float DistanceSq(Vector2 a, Vector2 b)
        {
            return (b - a).sqrMagnitude;
        }
    
        public static int FindClosest(Vector2 pos, ref List<GameObject> objects)
        {
            int closestIndex = 0;
            float closestDistance = DistanceSq(pos, objects[0].transform.position);
            for (int i = 1; i < objects.Count; ++i)
            {
                float dist = DistanceSq(pos, objects[i].transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestIndex = i;
                }
            }
            return closestIndex;
        }
    }
}
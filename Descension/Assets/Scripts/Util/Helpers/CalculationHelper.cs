using System.Collections.Generic;
using UnityEngine;

namespace Util.Helpers
{
    public static class CalculationHelper
    {
        public static float DistanceSq(Vector2 a, Vector2 b)
        {
            return (b - a).sqrMagnitude;
        }
    
        public static int FindClosest(Vector2 pos, List<Transform> objects)
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
        
        // wraps index if out of range
        public static int SafeIndex(int x, int length)
        {
            int index = x % length;
            return index < 0 ? index + length : index;
        }
    }
}
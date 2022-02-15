using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Util.Helpers
{
    public static class CalculationHelper
    {
        public static float DistanceSq(Vector2 a, Vector2 b)
        {
            return (b - a).sqrMagnitude;
        }
    
        public static int FindClosest(Vector2 pos, ref List<Transform> objects)
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
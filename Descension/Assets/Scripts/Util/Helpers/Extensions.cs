using System;
using UnityEngine;

namespace Util.Helpers
{
    public static class Extensions
    {
        public static Transform GetChildTransformWithName(this Transform transform, string name)
        {
            foreach(Transform t in transform)
                if (t.name == name)
                    return t;
            return null;
        }
        
        public static Transform GetChildTransformWithName(this GameObject gameObject, string name)
        {
            return GetChildTransformWithName(gameObject.transform, name);
        }
        
        public static GameObject GetChildObjectWithName(this Transform transform, string name)
        {
            foreach(Transform t in transform)
                if (t.name == name)
                    return t.gameObject;
            return null;
        }
        
        public static GameObject GetChildObjectWithName(this GameObject gameObject, string name)
        {
            return GetChildObjectWithName(gameObject.transform, name);
        }

        public static Vector2 GetRotated(this Vector2 vector, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
            
            return new Vector2(cos*vector.x - sin*vector.y, sin*vector.x + cos*vector.y);
        }
    }
}
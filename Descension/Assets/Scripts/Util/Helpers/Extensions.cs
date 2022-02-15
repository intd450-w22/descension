using UnityEngine;

namespace Assets.Scripts.Util.Helpers
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
    }
}
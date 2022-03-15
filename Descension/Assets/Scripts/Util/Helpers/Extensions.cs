using System;
using UnityEngine;

namespace Util.Helpers
{
    public static class Extensions
    {
        #region Transform

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

        #endregion

        #region GameObject

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

        #endregion

        #region Vector2

        public static Vector2 GetRotated(this Vector2 vector, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
            
            return new Vector2(cos*vector.x - sin*vector.y, sin*vector.x + cos*vector.y);
        }

        #endregion

        #region RectTransform

        public static float GetWidth(this RectTransform rt) =>
            rt.rect.width;

        public static void SetWidth(this RectTransform rt, float w) =>
            // rt.sizeDelta = new Vector2(w, rt.sizeDelta.y);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);

        public static float GetHeight(this RectTransform rt) =>
            rt.rect.height;

        public static void SetHeight(this RectTransform rt, float h) =>
            // rt.sizeDelta = new Vector2(rt.sizeDelta.x, h);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        public static float GetLeft(this RectTransform rt) =>
            rt.offsetMin.x;

        public static void SetLeft(this RectTransform rt, float left) =>
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);

        public static float GetRight(this RectTransform rt) =>
            -rt.offsetMax.x;

        public static void SetRight(this RectTransform rt, float right) =>
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);

        public static float GetTop(this RectTransform rt) =>
            -rt.offsetMax.y;

        public static void SetTop(this RectTransform rt, float top) =>
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        
        public static float GetBottom(this RectTransform rt) =>
            rt.offsetMin.y;

        public static void SetBottom(this RectTransform rt, float bottom) =>
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        
        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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

        public static void Enable(this GameObject gameObject) => gameObject.SetActive(true);
        public static void Disable(this GameObject gameObject) => gameObject.SetActive(false);
        public static bool IsEnabled(this GameObject gameObject) => gameObject.activeInHierarchy;

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
        
        public static Vector2 GetRotated(this Vector3 vector, float degrees)
        {
            return GetRotated((Vector2)vector, degrees);
        }

        public static float ToDegrees(this Vector3 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }
        
        public static float ToDegrees(this Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }

        public static Vector2 ToVector(this float angle)
        {
            double radians = angle * Mathf.Deg2Rad;
            return new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
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

        #region String

        public static string RemoveWhitespace(this string str) =>
            string.Concat(str.Where(c => !char.IsWhiteSpace(c)));

        public static bool IsNullOrEmpty([CanBeNull] this string str) => string.IsNullOrEmpty(str);

        #endregion

        #region IEnumerable

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return true;

            var collection = enumerable as ICollection<T>;
            if (collection != null)
                return collection.Count < 1;

            return !enumerable.Any();
        }

        #endregion

    }
}
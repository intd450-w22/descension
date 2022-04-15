using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Actor.Interface;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util.Helpers
{
    public static class Extensions
    {
        #region Transform

        public static Transform GetChildTransform(this Transform transform, string name)
        {
            foreach(Transform t in transform)
                if (t.name == name)
                    return t;
            return null;
        }
        
        public static Transform GetChildTransform(this GameObject gameObject, string name)
        {
            return GetChildTransform(gameObject.transform, name);
        }

        #endregion
        
        #region Monobehaviour

        public static void Invoke(this MonoBehaviour monoBehaviour, Action action, float delay)
        {
            monoBehaviour.StartCoroutine(InvokeRoutine(action, delay));
        }

        private static IEnumerator InvokeRoutine(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        public static void InvokeWhen(this MonoBehaviour monoBehaviour, Action action, Func<bool> when, float checkDelay = 0)
        {
            monoBehaviour.StartCoroutine(InvokeRoutineWhen(action, when, checkDelay));
        }
        
        private static IEnumerator InvokeRoutineWhen(Action invoke, Func<bool> when, float checkDelay)
        {
            while (true)
            {
                if (when())
                {
                    invoke();
                    yield break;
                }
                yield return new WaitForSeconds(checkDelay);
            }
        }
        
        #endregion

        #region Object
        
        public static T GetComponent<T>(this Object obj) where T : class
        {
            var gameObject = obj as GameObject;
            return gameObject != null ? gameObject.GetComponent<T>() : null;
        }
        
        #endregion
        
        #region GameObject

        public static T GetComponent<T>(this GameObject gameObject, bool getFromHierarchyIfNull)
        { 
            return gameObject.GetComponent<T>() ?? gameObject.GetComponentInParent<T>() ?? gameObject.GetComponentInChildren<T>();
        }
        
        public static void Enable(this GameObject gameObject) => gameObject.SetActive(true);
        public static void Disable(this GameObject gameObject) => gameObject.SetActive(false);
        public static bool IsEnabled(this GameObject gameObject) => gameObject.activeInHierarchy;

        public static GameObject GetChildObject(this Transform transform, string name)
        {
            foreach(Transform t in transform)
                if (t.name == name)
                    return t.gameObject;
            return null;
        }
        
        public static GameObject GetChildObject(this GameObject gameObject, string name)
        {
            return GetChildObject(gameObject.transform, name);
        }
        
        public static IEnumerable<Transform> GetChildTransforms(this GameObject obj)
        {
            return obj.transform.GetChildTransforms();
        }
        
        public static IEnumerable<Transform> GetChildTransforms(this Transform transform)
        {
            return transform.GetComponentsInChildren<Transform>().Where(t => t != transform);
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
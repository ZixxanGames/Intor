using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Extensions
{
    public static class TransformExtensions
    {
        public static IEnumerator MoveTo(this Transform transform, Vector3 endPos, float speed = 1.75f, Action actionAfterMove = default, bool isLocal = false)
        {
            var t = 0f;

            var (x1, y1, z1) = !isLocal ? transform.position : transform.localPosition;
            var (x2, y2, z2) = endPos;

            while (t <= 1f)
            {
                yield return null;

                var pos = new Vector3(Mathf.SmoothStep(x1, x2, t), Mathf.SmoothStep(y1, y2, t), Mathf.SmoothStep(z1, z2, t));

                if (!isLocal) transform.position = pos;
                else transform.localPosition = pos;

                t += speed * Time.unscaledDeltaTime;
            }

            actionAfterMove?.Invoke();
        }
        public static IEnumerator MoveTo(this Transform transform, Quaternion endRot, float speed = 1.75f, Action actionAfterMove = default)
        {
            var t = 0f;

            var (x0, y0, z0, w0) = transform.rotation;
            var (x, y, z, w) = endRot;

            while (t <= 1)
            {
                yield return null;

                transform.rotation = new Quaternion(Mathf.SmoothStep(x0, x, t), Mathf.SmoothStep(y0, y, t), Mathf.SmoothStep(z0, z, t), Mathf.SmoothStep(w0, w, t));

                t += speed * Time.unscaledDeltaTime;
            }

            actionAfterMove?.Invoke();
        }
        public static IEnumerator MoveTo(this Transform transform, Vector3 endPos, Quaternion endRot, float speed = 1.75f, Action actionAfterMove = default, bool isLocal = false)
        {
            var t = 0f;

            var (x1, y1, z1) = !isLocal ? transform.position : transform.localPosition;
            var (x2, y2, z2) = endPos;

            var (x0, y0, z0, w0) = transform.rotation;
            var (x, y, z, w) = endRot;

            var startRotation = transform.rotation;

            while (t <= 1)
            {
                yield return null;

                var pos = new Vector3(Mathf.SmoothStep(x1, x2, t), Mathf.SmoothStep(y1, y2, t), Mathf.SmoothStep(z1, z2, t));

                if (!isLocal) transform.position = pos;
                else transform.localPosition = pos;

                transform.rotation = new Quaternion(Mathf.SmoothStep(x0, x, t), Mathf.SmoothStep(y0, y, t), Mathf.SmoothStep(z0, z, t), Mathf.SmoothStep(w0, w, t));

                t += speed * Time.unscaledDeltaTime;
            }

            actionAfterMove?.Invoke();
        }

        public static IEnumerator RotateWhile(this Transform transform, Vector3 direction, Func<bool> predicate, Action actionAfterRotation = default)
        {
            direction = new Vector3(direction.x, direction.y, -direction.z);

            while (predicate())
            {
                transform.Rotate(direction * Time.unscaledDeltaTime);

                yield return null;
            }

            actionAfterRotation?.Invoke();
        }

        public static IEnumerator ScaleTo(this Transform transform, Vector3 endScale, float speed = 1.75f, Action actionAfterScale = default)
        {
            var (x1, y1, z1) = transform.localScale;
            var (x2, y2, z2) = endScale;

            var t = 0f;

            while (t <= 1)
            {
                yield return null;

                transform.localScale = new Vector3(Mathf.SmoothStep(x1, x2, t), Mathf.SmoothStep(y1, y2, t), Mathf.SmoothStep(z1, z2, t));

                t += speed * Time.unscaledDeltaTime;
            }

            actionAfterScale?.Invoke();
        }


        public static T GetChild<T>(this Transform transform) where T : Component
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<T>() is T component && component) return component;
            }

            return null;
        }

        public static List<T> GetChilds<T>(this Transform transform) where T : Component
        {
            if (typeof(T) == typeof(Transform)) return transform.GetChilds() as List<T>;

            var childs = new List<T>();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<T>() is T component && component) childs.Add(component);
            }

            return childs;
        }
        public static List<Transform> GetChilds(this Transform transform)
        {
            var childs = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++) childs.Add(transform.GetChild(i));

            return childs;
        }
    }
}

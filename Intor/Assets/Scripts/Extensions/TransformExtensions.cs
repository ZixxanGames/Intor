using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static IEnumerator MoveTo(this Transform transform, Vector3 endPos, float speed = 5f, float minDistance = 1f, Action actionAfterMove = default)
    {
        while (Vector3.Distance(transform.position, endPos) > minDistance)
        {
            yield return null;

            transform.position = Vector3.Lerp(transform.position, endPos, speed * Time.fixedDeltaTime);
        }

        actionAfterMove?.Invoke();
    }
    public static IEnumerator MoveTo(this Transform transform, Quaternion endRot, float speed = 5f, float minAngle = 1f, Action actionAfterMove = default)
    {
        while (Quaternion.Angle(transform.rotation, endRot) > minAngle)
        {
            yield return null;

            transform.rotation = Quaternion.Lerp(transform.rotation, endRot, speed * Time.fixedDeltaTime);
        }

        actionAfterMove?.Invoke();
    }
    public static IEnumerator MoveTo(this Transform transform, Vector3 endPos, Quaternion endRot, float speed = 5f, float minDistance = 1f, float minAngle = 1f, Action actionAfterMove = default)
    {
        while (Vector3.Distance(transform.position, endPos) > minDistance || Quaternion.Angle(transform.rotation, endRot) > minAngle)
        {
            yield return null;

            transform.position = Vector3.Lerp(transform.position, endPos, speed * Time.fixedDeltaTime);

            transform.rotation = Quaternion.Lerp(transform.rotation, endRot, speed * Time.fixedDeltaTime);
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

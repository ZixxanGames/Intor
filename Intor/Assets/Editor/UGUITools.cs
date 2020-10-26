using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class UGUITools : Editor
{
    [MenuItem("UGUI/Anchors to Corners #Q")]
    public static void AnchorsToCorners()
    {
        foreach (Transform transform in Selection.transforms)
        {
            if (!(transform is RectTransform rect) || !(Selection.activeTransform.parent is RectTransform parentRect)) return;

            Undo.RecordObject(transform, transform.name);

            Vector2 newAnchorsMin = new Vector2(rect.anchorMin.x + rect.offsetMin.x / parentRect.rect.width,
                                                rect.anchorMin.y + rect.offsetMin.y / parentRect.rect.height);

            Vector2 newAnchorsMax = new Vector2(rect.anchorMax.x + rect.offsetMax.x / parentRect.rect.width,
                                                rect.anchorMax.y + rect.offsetMax.y / parentRect.rect.height);

            rect.anchorMin = newAnchorsMin;
            rect.anchorMax = newAnchorsMax;
            rect.offsetMin = rect.offsetMax = new Vector2(0, 0);

			SetDirty(transform.gameObject);
		}
    }

    [MenuItem("UGUI/Corners to Anchors #E")]
    public static void CornersToAnchors()
    {
        foreach (Transform transform in Selection.transforms)
        {
            if (transform is RectTransform rect)
            {
                Undo.RecordObject(transform, transform.name);

                rect.offsetMin = rect.offsetMax = new Vector2(0, 0);

                SetDirty(transform.gameObject);
            }
        }
    }

	[MenuItem("UGUI/Mirror Horizontally Around Anchors #Z")]
	public static void MirrorHorizontallyAnchors() => MirrorHorizontally(false);

	[MenuItem("UGUI/Mirror Horizontally Around Parent Center #X")]
	public static void MirrorHorizontallyParent() => MirrorHorizontally(true);

	[MenuItem("UGUI/Mirror Vertically Around Anchors #A")]
	public static void MirrorVerticallyAnchors() => MirrorVertically(false);

	[MenuItem("UGUI/Mirror Vertically Around Parent Center #S")]
	public static void MirrorVerticallyParent() => MirrorVertically(true);


	private static void MirrorHorizontally(bool mirrorAnchors)
	{
		foreach (Transform transform in Selection.transforms)
		{
			if (!(transform is RectTransform rect)) return;

			Undo.RecordObject(transform, transform.name);

			if (mirrorAnchors)
			{
				Vector2 oldAnchorMin = rect.anchorMin;
				rect.anchorMin = new Vector2(1 - rect.anchorMax.x, rect.anchorMin.y);
				rect.anchorMax = new Vector2(1 - oldAnchorMin.x, rect.anchorMax.y);
			}

			Vector2 oldOffsetMin = rect.offsetMin;
			rect.offsetMin = new Vector2(-rect.offsetMax.x, rect.offsetMin.y);
			rect.offsetMax = new Vector2(-oldOffsetMin.x, rect.offsetMax.y);

			rect.localScale = new Vector3(-rect.localScale.x, rect.localScale.y, rect.localScale.z);

			SetDirty(transform.gameObject);
		}
	}

	private static void MirrorVertically(bool mirrorAnchors)
	{
		foreach (Transform transform in Selection.transforms)
		{
			if (!(transform is RectTransform rect)) return;

			Undo.RecordObject(transform, transform.name);

			if (mirrorAnchors)
			{
				Vector2 oldAnchorMin = rect.anchorMin;
				rect.anchorMin = new Vector2(rect.anchorMin.x, 1 - rect.anchorMax.y);
				rect.anchorMax = new Vector2(rect.anchorMax.x, 1 - oldAnchorMin.y);
			}

			Vector2 oldOffsetMin = rect.offsetMin;
			rect.offsetMin = new Vector2(rect.offsetMin.x, -rect.offsetMax.y);
			rect.offsetMax = new Vector2(rect.offsetMax.x, -oldOffsetMin.y);

			rect.localScale = new Vector3(rect.localScale.x, -rect.localScale.y, rect.localScale.z);

			SetDirty(transform.gameObject);
		}
	}


	private static void SetDirty(GameObject obj)
    {
        if (!GUI.changed || Application.isPlaying) return;

        EditorUtility.SetDirty(obj);
        EditorSceneManager.MarkSceneDirty(obj.scene);
    }
}

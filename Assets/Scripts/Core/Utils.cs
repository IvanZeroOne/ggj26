using UnityEngine;

public static class Utils
{
    public static void DestroyAllChildren(this Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            Object.Destroy(child.gameObject);
        }
        parentTransform.DetachChildren();
    }
}
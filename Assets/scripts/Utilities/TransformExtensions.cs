using UnityEngine;

public static class TransformExtensions {

    public static void DestroyChildren(this Transform t) {
        while (t.childCount > 0) {
            Object.Destroy(t.GetChild(0));
        }
    }
    
}

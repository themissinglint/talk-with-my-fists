using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationCurveExtensions {

    public static AnimationCurve Copy(this AnimationCurve curve) {
        AnimationCurve copy = new AnimationCurve();
        foreach (Keyframe kf in curve.keys) {
            copy.AddKey(kf);
        }
        return copy;
    }

    public static Keyframe LastKeyframe (this AnimationCurve curve) {
        return curve.keys[curve.keys.Length - 1];
    }

    public static float Duration(this AnimationCurve curve) {
        if (curve.keys == null || curve.keys.Length == 0) return 0f;
        return curve.LastKeyframe().time;
    }

    public static float FirstValue(this AnimationCurve curve) {
        if (curve.keys == null || curve.keys.Length == 0) return 0f;
        return curve.keys[0].value;
    }

    public static float LastValue(this AnimationCurve curve) {
        if (curve.keys == null || curve.keys.Length == 0) return 0f;
        return curve.LastKeyframe().value;
    }
    
}

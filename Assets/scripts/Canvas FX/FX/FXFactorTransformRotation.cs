using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXFactorTransformRotation : FXFactorTransform {
    
    public AnimationCurve AngleCurve;
    public float ValueMultiplier;

    public override bool IsExpired => Age >= AngleCurve.Duration();

    public override float GetRotation()  {
        float angleValue = AngleCurve.Evaluate(Age);
        foreach (AnimationCurve curve in Args.Curves) {
            angleValue *= curve.Evaluate(Age);
        }
        float multiplierProduct = ValueMultiplier * Args.Amplitude;
        return angleValue * multiplierProduct;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXFactorTransformSquash : FXFactorTransform {
    
    public AnimationCurve ValueCurve;
    public float ValueMultiplier;
    
    public override bool IsExpired => Age >= ValueCurve.Duration();

    public float GetCurrentValue() {
        float curveProduct = ValueCurve.Evaluate(Age);
        foreach (AnimationCurve curve in Args.Curves) {
            curveProduct *= curve.Evaluate(Age);
        }
        return curveProduct * ValueMultiplier * Args.Amplitude;
    }
    
    public override Vector2 GetSquashVector() {
        if (Args.InputVector != Vector2.zero) {
            Args.InputVector = Args.InputVector.normalized;
        }
        return GetCurrentValue() * Args.InputVector;
    }
    
}

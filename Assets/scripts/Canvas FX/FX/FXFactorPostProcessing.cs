using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FXFactorPostProcessing : FXFactor {

    public string VolumeID;
    public AnimationCurve ValueCurve;

    public override bool IsExpired => Age >= ValueCurve.Duration();

    public float GetValue() {
        float curveProduct = ValueCurve.Evaluate(Age);
        curveProduct = Args.Curves.Aggregate(curveProduct, (current, curve) => current * curve.Evaluate(Age));
        return curveProduct * Args.Amplitude;
    }
    
}
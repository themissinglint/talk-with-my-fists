using System.Linq;
using UnityEngine;

public class FXFactorTransformOffsetPulse : FXFactorTransform {

    public AnimationCurve ValueCurve;
    public float ValueMultiplier;

    public override bool IsExpired => Age >= ValueCurve.Duration();

    public override Vector2 GetOffset() {
        if (Args.InputVector == Vector2.zero) return base.GetOffset();
        float curveProduct = ValueCurve.Evaluate(Age);
        curveProduct = Args.Curves.Aggregate(curveProduct, (current, curve) => current * curve.Evaluate(Age));
        float multiplierProduct = ValueMultiplier * Args.Amplitude;
        return Args.InputVector * (curveProduct * multiplierProduct);
    }
    
}

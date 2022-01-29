using System.Linq;
using UnityEngine;

public class FXFactorTransformScalePulse : FXFactorTransform {

    public AnimationCurve ValueCurve;
    public float ValueMultiplier;

    public override bool IsExpired => Age >= ValueCurve.Duration();

    public override Vector2 GetScaleVector() {
        float curveProduct = ValueCurve.Evaluate(Age);
        curveProduct = Args.Curves.Aggregate(curveProduct, (current, curve) => current * curve.Evaluate(Age));
        float multiplierProduct = ValueMultiplier * Args.Amplitude;
        return Vector2.one * (curveProduct * multiplierProduct);
    }
    
}

using UnityEngine;

public class FXFactorColorPulse : FXFactorColor {

    public Color Color;
    public AnimationCurve ValueCurve;
    public float ValueMultiplier;

    public override bool IsExpired => Age >= ValueCurve.Duration();

    public override Color GetColor() {
        return Color;
    }

    public override float GetStrength() {
        float curveProduct = ValueCurve.Evaluate(Age);
        foreach (AnimationCurve curve in Args.Curves) {
            curveProduct *= curve.Evaluate(Age);
        }
        float multiplierProduct = ValueMultiplier * Args.Amplitude;
        return curveProduct * multiplierProduct;
    }
}

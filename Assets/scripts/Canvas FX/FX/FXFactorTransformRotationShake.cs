using UnityEngine;

public class FXFactorTransformRotationShake : FXFactorTransform {
    
    public AnimationCurve ValueCurve;
    public float ValueMultiplier;
    public float PerlinSpeed;

    public override bool IsExpired => Age >= ValueCurve.Duration();

    private readonly Vector2 _seedVector;

    public FXFactorTransformRotationShake() {
        _seedVector = new Vector2(Random.Range(0f, 1000f), Random.Range(0f, 1000f));
    }

    public override float GetRotation()  {
        float curveProduct = ValueCurve.Evaluate(Age);
        foreach (AnimationCurve curve in Args.Curves) {
            curveProduct *= curve.Evaluate(Age);
        }
        float multiplierProduct = ValueMultiplier * Args.Amplitude;
        float dir = Mathf.PerlinNoise(_seedVector.x + Age * PerlinSpeed, _seedVector.y) - 0.5f;
        return dir * curveProduct * multiplierProduct;
    }
    
}

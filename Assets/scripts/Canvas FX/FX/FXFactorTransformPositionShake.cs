using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class FXFactorTransformPositionShake : FXFactorTransform {
    
    public AnimationCurve ValueCurve;
    public float ValueMultiplier;
    public float PerlinSpeed;

    public override bool IsExpired => Age >= ValueCurve.Duration();

    private readonly Vector2 _seedVectorX;
    private readonly Vector2 _seedVectorY;
    private readonly Vector2 _seedVectorMag;

    public FXFactorTransformPositionShake() {
        _seedVectorX = new Vector2(Random.Range(0f, 1000f), Random.Range(0f, 1000f));
        _seedVectorY = new Vector2(Random.Range(0f, 1000f), Random.Range(0f, 1000f));
        _seedVectorMag = new Vector2(Random.Range(0f, 1000f), Random.Range(0f, 1000f));
    }
    
    public override Vector2 GetOffset() {
        float curveProduct = ValueCurve.Evaluate(Age);
        foreach (AnimationCurve curve in Args.Curves) {
            curveProduct *= curve.Evaluate(Age);
        }
        float multiplierProduct = ValueMultiplier * Args.Amplitude;
        Vector2 dir = new Vector2(Mathf.PerlinNoise(_seedVectorX.x + Age * PerlinSpeed, _seedVectorX.y) - 0.5f, Mathf.PerlinNoise(_seedVectorY.x + Age * PerlinSpeed, _seedVectorY.y) - 0.5f);
        Vector2 offsetVector = dir.normalized * Mathf.PerlinNoise(_seedVectorMag.x + Age * PerlinSpeed, _seedVectorMag.y);
        return offsetVector * curveProduct * multiplierProduct;
    }
    
}

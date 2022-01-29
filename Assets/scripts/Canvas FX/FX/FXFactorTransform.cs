using UnityEngine;

public class FXFactorTransform : FXFactor {
    
    public virtual Vector2 GetOffset() {
        return Vector2.zero;
    }

    public virtual Vector2 GetScaleVector() {
        return Vector2.zero;
    }

    public virtual float GetRotation() {
        return 0f;
    }

    public virtual Vector2 GetSquashVector() {
        return Vector2.zero;
    }

}

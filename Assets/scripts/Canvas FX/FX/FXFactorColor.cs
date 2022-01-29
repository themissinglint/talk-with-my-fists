using UnityEngine;

public class FXFactorColor : FXFactor {

    public Material MaterialReference;

    public virtual Color GetColor() {
        return Color.black;
    }

    public virtual float GetStrength() {
        return 0f;
    }
    
}

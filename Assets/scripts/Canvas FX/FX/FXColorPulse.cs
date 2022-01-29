using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FXColorPulse : FXColor {

    public Color Color;
    public AnimationCurve ValueCurve;
    public float ValueMultiplier = 1f;
    
    public override void Trigger(FXArgs args) {
        foreach (FXApplierColor applier in targetAppliers) {
            applier.AddFactor(BuildFactor(args));
        }
    }

    private FXFactorColorPulse BuildFactor(FXArgs args) {
        FXFactorColorPulse newFactor = new FXFactorColorPulse {
            StartTime = Time.time,
            Source = this,
            Args = args,
            ValueCurve = ValueCurve,
            ValueMultiplier = ValueMultiplier,
            Color = Color,
            MaterialReference = Material
        };
        return newFactor;
    }
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(FXColorPulse))]
[CanEditMultipleObjects]
public class FXColorPulseEditor : Editor {

    void OnEnable() {
        FXColorPulse script = (FXColorPulse)target;
        script.Material = Resources.Load<Material>("FX Color Additive");
    }
    
}
#endif

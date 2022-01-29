using UnityEngine;

public class FXTransformScalePulse : FXTransform {

    public AnimationCurve ValueCurve;
    public float ValueMultiplier = 1f;
    
    public override void Trigger(FXArgs args) {
        foreach (FXApplierTransform applier in targetAppliers) {
            applier.AddFactor(BuildFactor(args));
        }
    }

    private FXFactorTransformScalePulse BuildFactor(FXArgs args) {
        FXFactorTransformScalePulse newFactor = new FXFactorTransformScalePulse {
            StartTime = Time.time,
            Source = this,
            Args = args,
            ValueCurve = ValueCurve,
            ValueMultiplier = ValueMultiplier
        };
        return newFactor;
    }

}

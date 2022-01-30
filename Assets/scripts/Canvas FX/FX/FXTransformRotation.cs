using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXTransformRotation : FXTransform {
    
    public AnimationCurve AngleCurve;
    public float ValueMultiplier = 1f;
    
    public override void Trigger(FXArgs args) {
        foreach (FXApplierTransform applier in targetAppliers) {
            applier.AddFactor(BuildFactor(args));
        }
    }

    private FXFactorTransformRotation BuildFactor(FXArgs args) {
        FXFactorTransformRotation newFactor = new FXFactorTransformRotation {
            StartTime = Time.time,
            Source = this,
            Args = args,
            AngleCurve = AngleCurve,
            ValueMultiplier = ValueMultiplier
        };
        return newFactor;
    }
    
}

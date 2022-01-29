using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXTransformOffsetPulse : FXTransform {

    public AnimationCurve ValueCurve;
    public float ValueMultiplier = 1f;
    
    public override void Trigger(FXArgs args) {
        foreach (FXApplierTransform applier in targetAppliers) {
            applier.AddFactor(BuildFactor(args));
        }
    }

    private FXFactorTransformOffsetPulse BuildFactor(FXArgs args) {
        FXFactorTransformOffsetPulse newFactor = new FXFactorTransformOffsetPulse {
            StartTime = Time.time,
            Source = this,
            Args = args,
            ValueCurve = ValueCurve,
            ValueMultiplier = ValueMultiplier
        };
        return newFactor;
    }

}

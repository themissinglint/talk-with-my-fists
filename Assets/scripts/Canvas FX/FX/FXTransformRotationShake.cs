using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXTransformRotationShake : FXTransform {
    
    public AnimationCurve ValueCurve;
    public float ValueMultiplier = 0.1f;
    public float PerlinSpeed = 15f;
    
    public override void Trigger(FXArgs args) {
        foreach (FXApplierTransform applier in targetAppliers) {
            applier.AddFactor(BuildFactor(args));
        }
    }

    private FXFactorTransformRotationShake BuildFactor(FXArgs args) {
        FXFactorTransformRotationShake newFactor = new FXFactorTransformRotationShake {
            StartTime = Time.time,
            Source = this,
            Args = args,
            ValueCurve = ValueCurve,
            ValueMultiplier = ValueMultiplier,
            PerlinSpeed = PerlinSpeed
        };
        return newFactor;
    }
    
}
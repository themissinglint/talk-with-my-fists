using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXPostProcessing : FX {

    public string VolumeID;
    public AnimationCurve ValueCurve;
    
    public override void Trigger(FXArgs args) {
        PostProcessingManager.Instance.AddFireAndForgetFactor(BuildFactor(args));
    }
    
    private FXFactorPostProcessing BuildFactor(FXArgs args) {
        FXFactorPostProcessing newFactor = new FXFactorPostProcessing {
            VolumeID = VolumeID,
            StartTime = Time.time,
            Source = this,
            Args = args,
            ValueCurve = ValueCurve
        };
        return newFactor;
    }
    
}

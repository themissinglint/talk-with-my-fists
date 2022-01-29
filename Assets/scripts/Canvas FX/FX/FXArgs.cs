using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FXArgs {

    private Action<FXArgs> _triggerAction;
    public float Amplitude = 1f;
    public List<AnimationCurve> Curves = new List<AnimationCurve>();
    public float Speed = 1f;
    public Vector2 InputVector = Vector2.zero;
    public Vector2 OriginPointOverride;

    public FXArgs() {
        
    }
    
    public FXArgs(Action<FXArgs> triggerAction) {
        _triggerAction = triggerAction;
    }

    public FXArgs WithAmplitude(float amplitude) {
        Amplitude = amplitude;
        return this;
    }

    public FXArgs WithCurve(AnimationCurve curve) {
        Curves.Add(curve.Copy());
        return this;
    }

    public FXArgs WithSpeed(float speed) {
        Speed = speed;
        return this;
    }

    public FXArgs WithInputVector(Vector2 inputVector) {
        InputVector = inputVector;
        return this;
    }

    public FXArgs WithOriginPoint(Vector2 originPoint) {
        OriginPointOverride = originPoint;
        return this;
    }
    
    public void AndTrigger() {
        _triggerAction.Invoke(this);
    }
    
    
    
}

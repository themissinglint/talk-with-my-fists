using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXFactor {
    
    public float StartTime;
    public object Source;
    public FXArgs Args;

    protected float Age => (Time.time - StartTime) * Args.Speed;
    public virtual bool IsExpired => true;

}

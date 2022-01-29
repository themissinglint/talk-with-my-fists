using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StandardFXInvoker : MonoBehaviour {

    public string FXName;
    public GameObject TargetObject;
    public float Amplitude = 1f;
    public bool SetOriginPoint = false;
    
    public bool InvokeOnEnable;

    private void OnEnable() {
        if (InvokeOnEnable) {
            DoInvoke();
        }
    }
    
    public void DoInvoke() {
        FXArgs args = new FXArgs {Amplitude = Amplitude};
        if (SetOriginPoint) {
            args.OriginPointOverride = transform.position;
        }
        StandardFX.DoEffect(FXName, TargetObject, args);
    }
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(FXColorPulse))]
[CanEditMultipleObjects]
public class StandardFXInvokerEditor : Editor {

    void OnEnable() {
        StandardFXInvoker script = (StandardFXInvoker)target;
        if (script.TargetObject == null) {
            script.TargetObject = script.gameObject;
        }
    }
    
}
#endif
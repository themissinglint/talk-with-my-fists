using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class FXColor : FX {

    public Material Material;
    
    protected List<FXApplierColor> targetAppliers;
    
    public override bool SetTargets(List<GameObject> targetObjects) {
        bool anyChange = base.SetTargets(targetObjects);
        if (!anyChange) {
            return false;
        }
        targetAppliers = new List<FXApplierColor>();
        foreach (GameObject go in targetObjects) {
            FXApplierColor applier = go.GetComponent<FXApplierColor>();
            if (applier != null) {
                targetAppliers.AddIfUnique(applier);
            }
            else {
                applier = go.AddComponent<FXApplierColor>();
                targetAppliers.Add(applier);
            }
        }
        return true;
    }
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(FXColor))]
[CanEditMultipleObjects]
public class FXColorEditor : Editor {

    void OnEnable() {
        FXColor script = (FXColor)target;
        script.Material = Resources.Load<Material>("FX Color Additive");
    }
    
}
#endif

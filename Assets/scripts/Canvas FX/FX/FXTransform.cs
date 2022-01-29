using System.Collections.Generic;
using UnityEngine;

public class FXTransform : FX {
    
    protected List<FXApplierTransform> targetAppliers;
    
    public override bool SetTargets(List<GameObject> targetObjects) {
        bool anyChange = base.SetTargets(targetObjects);
        if (!anyChange) {
            return false;
        }
        targetAppliers = new List<FXApplierTransform>();
        foreach (GameObject go in targetObjects) {
            FXApplierTransform applier = go.GetComponent<FXApplierTransform>();
            if (applier != null) {
                targetAppliers.AddIfUnique(applier);
            }
            else {
                applier = go.AddComponent<FXApplierTransform>();
                targetAppliers.Add(applier);
            }
        }
        return true;
    }

}

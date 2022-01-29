using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FX : MonoBehaviour {
    
    private List<GameObject> installedObjects = new List<GameObject>();
    
    public virtual bool SetTargets(List<GameObject> targetObjects) {
        if (installedObjects.SequenceEqual(targetObjects)) {
            return false;
        }
        installedObjects = targetObjects.ToList();
        return true;
    }
    
    public virtual void Trigger(FXArgs args) {
        
    }
    
}

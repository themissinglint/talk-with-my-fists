using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class FXController : MonoBehaviour {

    public List<GameObject> Targets;
    private List<FX> _fxList;

    private void Awake() {
        _fxList = GetComponents<FX>().ToList();
    }

    [Button("Trigger")]
    public void Trigger() {
        Trigger(new FXArgs(Trigger));
    }
    
    private void Trigger(FXArgs args) {
        foreach (FX fx in _fxList) {
            fx.SetTargets(Targets);
            fx.Trigger(args);
        }
    }

    public FXArgs BuildCustom() {
        return new FXArgs(Trigger);
    }

    public void TriggerWithArgs(FXArgs args) {
        Trigger(args);
    }

}

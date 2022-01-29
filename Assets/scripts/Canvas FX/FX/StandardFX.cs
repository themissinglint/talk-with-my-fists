using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// References a number of pre-made graphical effects that can be invoked anywhere within the game. Convenient for
/// calling attention to UI elements when interaction is being called for.
/// </summary>
public class StandardFX : MonoBehaviour {
    
    private static StandardFX Instance;
    
    [Serializable]
    public class UIObjectFXMapping {
        public string EffectName;
        public GameObject Prefab;
    }

    private class UIObjectFXMappingInternal {
        public readonly string EffectName;
        private readonly GameObject _prefab;
        private readonly Transform _transform;
        private FXController _controller;
        public FXController Controller {
            get {
                if (_transform == null) return null;
                if (_controller == null) {
                    _controller = Instantiate(_prefab, _transform).GetComponent<FXController>();
                }
                return _controller;
            }
        }

        public UIObjectFXMappingInternal(UIObjectFXMapping mapping, Transform parentTransform) {
            EffectName = mapping.EffectName;
            _prefab = mapping.Prefab;
            _transform = parentTransform;
        }
    }

    public List<UIObjectFXMapping> EffectMappings = new List<UIObjectFXMapping>();
    private List<UIObjectFXMappingInternal> _mappings;

    private void Awake() {
        Instance = this;
    }
    
    private void Start() {
        _mappings = new List<UIObjectFXMappingInternal>();
        foreach (UIObjectFXMapping mapping in EffectMappings) {
            _mappings.Add(new UIObjectFXMappingInternal(mapping, transform));
        }
    }

    public static void DoEffect(string effectName, GameObject targetObject, FXArgs args) {
        if (!Instance) return;
        SetTarget(effectName, targetObject, out UIObjectFXMappingInternal effect);
        effect.Controller.TriggerWithArgs(args);
    }

    private static void SetTarget(string effectName, GameObject targetObject, out UIObjectFXMappingInternal effect) {
        effect = Instance._mappings.FirstOrDefault(e => e.EffectName == effectName);
        if (effect == null) {
            Debug.Log("No effect object found.");
            return;
        }
        effect.Controller.Targets = new List<GameObject>{targetObject};
    }

}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// References a number of pre-made graphical effects that can be invoked anywhere within the game. Convenient for
/// calling attention to UI elements when interaction is being called for.
/// </summary>
public class UIObjectFX : MonoBehaviour {
    
    private static UIObjectFX instance;

    public enum EffectType {
        
        // Scale Pulse to pull the eye.
        AttributeGainedPulse,
        
        // Squashy Bounce.
        NPCPerkUpPulse
        
    }

    /// <summary>
    /// Holds a mapping from an EffectType to an FX Prefab or instantiated FXPackageController. If a controller is
    /// called for but does not exist, a new controller object will be instantiated and cached.
    /// </summary>
    private class UIObjectFXMappingInternal {
        public readonly EffectType EffectType;
        private readonly GameObject _prefab;
        private readonly Transform _transform;
        private FXController _controller;
        public FXController Controller {
            get {
                if (!_transform) return null;
                if (!_controller) {
                    _controller = Instantiate(_prefab, _transform).GetComponent<FXController>();
                }
                return _controller;
            }
        }

        public UIObjectFXMappingInternal(UIObjectFXMapping mapping, Transform parentTransform) {
            EffectType = mapping.EffectType;
            _prefab = mapping.Prefab;
            _transform = parentTransform;
        }
    }

    public List<UIObjectFXMapping> EffectMappings = new List<UIObjectFXMapping>();
    private List<UIObjectFXMappingInternal> _mappings;

    private void Awake() {
        instance = this;
    }
    
    private void Start() {
        _mappings = new List<UIObjectFXMappingInternal>();
        foreach (UIObjectFXMapping mapping in EffectMappings) {
            _mappings.Add(new UIObjectFXMappingInternal(mapping, transform));
        }
    }

    public static void DoEffect(EffectType effectType, GameObject targetObject, float amplitude = 1f, float timeOffset = 0f) {
        if (!instance) return;
        SetTarget(effectType, targetObject, out UIObjectFXMappingInternal effect);
        effect.Controller.TriggerWithArgs(new FXArgs{Amplitude = amplitude, InputVector = Vector2.up});
    }

    // TODO: Toggle.
    // public static void ToggleEffect(EffectType effectType, GameObject targetObject, bool isOn, float amplitude = 1f,  float timeOffset = 0f) {
    //     if (!instance) return;
    //     SetTarget(effectType, targetObject, out UIObjectFXMappingInternal effect);
    //     effect.Controller.ToggleAll(isOn, amplitude: amplitude, timeOffset: timeOffset);
    // }

    private static void SetTarget(EffectType effectType, GameObject targetObject, out UIObjectFXMappingInternal effect) {
        effect = instance._mappings.FirstOrDefault(e => e.EffectType == effectType);
        if (effect == null) {
            Debug.Log($"Error in {typeof(UIObjectFX)}: no effect found with {typeof(EffectType)} \"{effectType.ToString()}\"");
            return;
        }

        effect.Controller.gameObject.transform.position = targetObject.transform.position;
        effect.Controller.Targets = new List<GameObject>() {targetObject};
    }

}

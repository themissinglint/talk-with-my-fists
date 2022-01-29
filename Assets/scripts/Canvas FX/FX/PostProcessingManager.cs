using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour {

    public static PostProcessingManager Instance;

    public List<PostProcessingVolumeRef> Volumes = new List<PostProcessingVolumeRef>();
    private List<FXFactorPostProcessing> _fireAndForgetFactors = new List<FXFactorPostProcessing>();
    private Dictionary<object, FXFactorPostProcessing> _lastingFactors = new Dictionary<object, FXFactorPostProcessing>();

    private void Awake() {
        Instance = this;
    }

    public void AddFireAndForgetFactor(FXFactorPostProcessing newFactor) {
        _fireAndForgetFactors.Add(newFactor);
    }

    public void SetLastingFactor (object factorProducer, FXFactorPostProcessing newFactor) {
        if (!_lastingFactors.ContainsKey(factorProducer)) {
            _lastingFactors.Add(factorProducer, newFactor);
        }
    }

    public void RemoveLastingFactor(object factorProducer) {
        if (_lastingFactors.ContainsKey(factorProducer)) {
            _lastingFactors.Remove(factorProducer);
        }
    }

    private void Update() {

        List<FXFactorPostProcessing> allFactors = _fireAndForgetFactors.ToList();
        allFactors.AddRange(_lastingFactors.Values);

        // Set each PostProcessVolume's weight to be the max among its factors, divided by maxStrength.
        foreach (PostProcessingVolumeRef vol in Volumes) {
            List<FXFactorPostProcessing> factors = allFactors.Where(e => e.VolumeID == vol.VolumeID).ToList();
            vol.Ref.weight = factors.Count == 0 ? 0f : factors.Max(e => e.GetValue());
        }
        
        // Age factors and remove outdated ones.
        for (int i = _fireAndForgetFactors.Count - 1; i >= 0; i--) {
            if (_fireAndForgetFactors[i].IsExpired) _fireAndForgetFactors.RemoveAt(i);
        }
        
    }

}

[System.Serializable]
public class PostProcessingVolumeRef {

    public string VolumeID;
    public PostProcessVolume Ref;

}
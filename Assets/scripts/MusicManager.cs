using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioSource NeutralSource;
    public AudioLowPassFilter NeutralLowPass;
    public float NeutralMax;
    public float NeutralMin;
    private float _neutralValueCurrent;
    private float _neutralValueVelocity;
    public AnimationCurve LowPassHzByDayProgress;

    public float DampingTime;

    public List<MusicLayer> Layers;

    private void Start() {
        _neutralValueCurrent = NeutralMax;
    }
    
    private void Update() {
        float avgLayerVolume = Layers.Average(e => e.ValueCurrent);
        float neutralTargetVolume = Mathf.Lerp(NeutralMax, NeutralMin, avgLayerVolume);
        _neutralValueCurrent = Mathf.SmoothDamp(_neutralValueCurrent, neutralTargetVolume, ref _neutralValueVelocity, DampingTime);
        NeutralSource.volume = _neutralValueCurrent;
        NeutralLowPass.cutoffFrequency = LowPassHzByDayProgress.Evaluate(WorldStatus.GameTimeProgress);

        foreach (MusicLayer layer in Layers) {
            float targetValue = Mathf.Floor(PlayerStatus.StatValues[layer.Stat] * 4) / 12f; // Step up through four volume levels.
            layer.ValueCurrent = Mathf.SmoothDamp(layer.ValueCurrent, targetValue, ref layer.ValueVelocity, DampingTime);
            layer.Source.volume = layer.ValueCurrent;
            layer.LowPass.cutoffFrequency = LowPassHzByDayProgress.Evaluate(WorldStatus.GameTimeProgress);
        }
    }

}

[System.Serializable]
public class MusicLayer {

    public AudioSource Source;
    public PlayerStat Stat;
    public AudioLowPassFilter LowPass;
    [HideInInspector] public float ValueCurrent;
    [HideInInspector] public float ValueVelocity;

}

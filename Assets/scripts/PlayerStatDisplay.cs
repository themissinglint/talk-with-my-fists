using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatDisplay : MonoBehaviour {

    public List<StatWedge> Wedges;
    public float MaxAngle = 0.23f;
    public float FillDampTime;
    public float PulseMagnitude = 0.6f;

    [Space]
    [Header("Testing Only!")]
    public PlayerStat TestIncreasedStat;
    public float TestIncreaseAmount;

    private bool _firstUpdate = true;
    
    private void FirstUpdate() {
        _firstUpdate = false;
        foreach (StatWedge statWedge in Wedges) { 
            UIObjectFX.DoEffect(UIObjectFX.EffectType.AttributeGainedPulse, statWedge.GameObject, 0.1f);
        }
    }
    
    [Button("Test Increased Stat")]
    public void TestIncreaseStat() {
        StatWedge wedge = Wedges.First(e => e.Stat == TestIncreasedStat);
        wedge.CurProgress = Mathf.Clamp01(wedge.CurProgress + TestIncreaseAmount);
        UIObjectFX.DoEffect(UIObjectFX.EffectType.AttributeGainedPulse, wedge.GameObject, PulseMagnitude);
    }

    private void Update() {
        if (_firstUpdate) FirstUpdate();
        foreach (StatWedge statWedge in Wedges) {
            statWedge.DisplayProgress = Mathf.SmoothDamp(statWedge.DisplayProgress, statWedge.CurProgress, ref statWedge.ProgressVelocity, FillDampTime);
            statWedge.ColorImage.fillAmount = Mathf.Lerp(0f, MaxAngle, statWedge.DisplayProgress);
        }
    }

}

[System.Serializable]
public class StatWedge {
    public PlayerStat Stat;
    public GameObject GameObject;
    public Image ColorImage;
    [HideInInspector] public float CurProgress;
    [HideInInspector] public float DisplayProgress;
    [HideInInspector] public float ProgressVelocity;
}

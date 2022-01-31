using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.CorgiEngine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour {

    public CameraController CameraController;
    public List<BackgroundLayer> BackgroundLayers;
    public PostProcessVolume NightPPV;
    public PostProcessVolume DoomPPV;

    public AnimationCurve DoomCurve;

    [Header("Game Loss")]
    public float WhiteCoverColorTime;
    public Gradient WhiteCoverColorByTimeSinceGameEnd;
    public Image WhiteCover;
    
    
    private Vector2 _lastPos;
    
    private void Start() {
        _lastPos = transform.position;
        foreach (BackgroundLayer layer in BackgroundLayers) {
            layer.SpriteRenderers = layer.Layer.GetComponentsInChildren<SpriteRenderer>().ToList();
            SpriteRenderer sr = layer.Layer.GetComponent<SpriteRenderer>();
            if (sr != null) layer.SpriteRenderers.Add(sr);
        }
    }
    
    private void Update() {
        Vector2 delta = (Vector2) transform.position - _lastPos;
        foreach (BackgroundLayer layer in BackgroundLayers) {
            layer.Layer.Translate(-delta * layer.MotionMultiplier);
            foreach (SpriteRenderer sr in layer.SpriteRenderers) {
                sr.color = layer.DayNightGradient.Evaluate(1f - WorldStatus.GameTimeProgress);
            }
        }
        _lastPos = transform.position;
        NightPPV.weight = WorldStatus.GameTimeProgress;
        float doomProgress = Mathf.Clamp01((20f - WorldStatus.GameTimeLeft) / 20f);
        float doomValue = DoomCurve.Evaluate(doomProgress);
        DoomPPV.weight = doomValue;
        CameraController.Shake(new Vector3(doomValue * .4f, 0.25f, 1f));
        WhiteCover.color = WhiteCoverColorByTimeSinceGameEnd.Evaluate(WorldStatus.TimeSinceGameEnd / WhiteCoverColorTime);
        if (WorldStatus.TimeSinceGameEnd > 6.5f) {
            Application.Quit();
        }
    }

}

[System.Serializable]
public class BackgroundLayer {

    public Transform Layer;
    public float MotionMultiplier;
    
    [HideInInspector] public List<SpriteRenderer> SpriteRenderers;
    public Gradient DayNightGradient;

}

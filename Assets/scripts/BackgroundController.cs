using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

    public List<BackgroundLayer> BackgroundLayers;
    

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
    }

}

[System.Serializable]
public class BackgroundLayer {

    public Transform Layer;
    public float MotionMultiplier;
    
    [HideInInspector] public List<SpriteRenderer> SpriteRenderers;
    public Gradient DayNightGradient;

}

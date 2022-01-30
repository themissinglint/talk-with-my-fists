using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

    public List<BackgroundLayer> BackgroundLayers;

    private Vector2 _lastPos;

    private void Start() {
        _lastPos = transform.position;
    }
    
    private void Update() {
        Vector2 delta = (Vector2) transform.position - _lastPos;
        foreach (BackgroundLayer layer in BackgroundLayers) {
            layer.Layer.Translate(-delta * layer.MotionMultiplier);
        }
        _lastPos = transform.position;
    }

}

[System.Serializable]
public class BackgroundLayer {

    public Transform Layer;
    public float MotionMultiplier;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	public SpriteRenderer[] layers;
	public SpriteRenderer[] dayLayers;
	public Vector3[] moveRates;
	public Camera mainCamera;
	public float daySeconds = 20f;
	private Vector3 camStartPos;
	private Vector3[] layerStartPoses;
	private float time;


    // Start is called before the first frame update
    void Start()
    {
		camStartPos = mainCamera.transform.position;
		layerStartPoses = new Vector3[layers.Length];
		for(int i=0; i<layers.Length; i++) {
			layerStartPoses[i] = layers[i].transform.position;
		}
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<layers.Length; i++) {
			Vector3 pos = (mainCamera.transform.position - camStartPos);
			pos.x *= moveRates[i].x;
			pos.y *= moveRates[i].y;
			pos = pos + layerStartPoses[i];
			layers[i].transform.position = pos;
		}

		//TODO: use some global time.
		time += Time.deltaTime;
		if (time > daySeconds) {
			time -= daySeconds;
		}
		float alpha = Mathf.Sin(time / daySeconds * Mathf.PI * 2);
		for(int i=0; i < dayLayers.Length; i++) {
			Color tint = dayLayers[i].color;
			tint.a = alpha;
			dayLayers[i].color = tint;
		}
	}
}

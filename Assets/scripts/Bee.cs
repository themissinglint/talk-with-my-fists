using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
	private Transform[] patrolPath;
	public float speed;
	private int patrolPointIdx;
	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
	private Vector3[] patrolPoints;

	// Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		patrolPath = GetComponentsInChildren<Transform>();
		patrolPoints = new Vector3[patrolPath.Length];
		for(int i=0; i<patrolPath.Length; i++) {
			patrolPoints[i] = patrolPath[i].position;
		}
	}

	// Update is called once per frame
	void Update() {

		rb.AddForce((patrolPoints[patrolPointIdx] - transform.position).normalized * speed);

		if((transform.position - patrolPoints[patrolPointIdx]).sqrMagnitude < .8f * .8f) {
			patrolPointIdx = (patrolPointIdx + 1) % patrolPoints.Length;
		}

		// flip sprite towards target:
		spriteRenderer.flipX = transform.position.x < patrolPoints[patrolPointIdx].x;
	}
}

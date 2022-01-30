using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
	private Transform[] patrolPath;
	public float speed;
	public AudioClip beeSting, beeDies;
	public GameObject deathsplosion;
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

		if((transform.position - patrolPoints[patrolPointIdx]).sqrMagnitude < .4f * .4f) {
			patrolPointIdx = (patrolPointIdx + 1) % patrolPoints.Length;
		}

		// flip sprite towards target:
		spriteRenderer.flipX = transform.position.x < patrolPoints[patrolPointIdx].x;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.layer == 9) { // 9 is player

			Character plrChar = collision.gameObject.GetComponent<Character>();
			if(plrChar.MovementState.CurrentState == CharacterStates.MovementStates.Dashing) {
				// player hurts bee
				AudioSource.PlayClipAtPoint(beeDies, transform.position);
				Instantiate(deathsplosion, transform.position, Quaternion.identity);
				Destroy(gameObject);

			} else {
				// bee hurts player
				AudioSource.PlayClipAtPoint(beeSting, transform.position);
				plrChar.GetComponent<PlayerDamage>().TakeDamage(gameObject);
			}
		}
	}
}

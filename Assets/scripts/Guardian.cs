using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian: MonoBehaviour
{
	public Transform home, charge, retreat;
	public float speed;
	private int patrolPointIdx;
	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
	private Vector3[] patrolPoints;
	private Vector3 chargeTarget;
	private Vector3 homeTarget;
	private Vector3 retreatTarget;
	private EnemyDamage dmgStats;
	private int lastHP;
	private bool charging;
	private bool retreating;

	// Start is called before the first frame update
	void Start() {

		dmgStats = GetComponent<EnemyDamage>();
		lastHP = dmgStats.hp;

		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		homeTarget = home.position;
		chargeTarget = charge.position;
		retreatTarget = retreat.position;
	}

	// Update is called once per frame
	void Update() {

		Vector3 target = homeTarget;

		if (!charging && PlayerStatus.StatValues[PlayerStat.Social] > .8f) {
			retreating = true;
		}

		if (dmgStats.hp < lastHP) {
			charging = true;
			lastHP = dmgStats.hp;
		}

		if (charging) {
			target = chargeTarget;
		} else if(retreating) {
			target = retreatTarget;
		}

		// this is all in 1D (just X):
		rb.AddForce(new Vector3(Mathf.Sign(target.x - transform.position.x), 0f, 0f) * speed);

		if(charging && Mathf.Abs(transform.position.x - chargeTarget.x) < .4f) {
			charging = false;
		}
		if (lastHP < 8 && Mathf.Abs(transform.position.x - homeTarget.x) < .4f) {
			charging = true;
		}
		// flip sprite towards target:
		spriteRenderer.flipX = transform.position.x < patrolPoints[patrolPointIdx].x;
	}

}

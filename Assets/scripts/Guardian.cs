using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian: MonoBehaviour
{
	public Transform home, charge, retreat;
	public float speed;
	public int hpToRampage = 10;
	private int patrolPointIdx;
	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
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

		//these take the position at game start time:
		homeTarget = home.position;
		chargeTarget = charge.position;
		retreatTarget = retreat.position;
	}

	// Update is called once per frame
	void Update() {

		// default goal is home:
		Vector3 target = homeTarget;

		//TODO: make this not happen until player is watching.
		if (!charging && PlayerStatus.StatValues[PlayerStat.Social] > .8f) {
			retreating = true;
		}

		// charge if HP dropped:
		if (dmgStats.hp < lastHP) {
			charging = true;
			lastHP = dmgStats.hp;
		}

		// note charging/retreating may have been set in previous frames.
		if (charging) {
			target = chargeTarget;
		} else if(retreating) {
			target = retreatTarget;
		}

		//Note this is all in 1D (just X):

		//Move towards target:
		rb.AddForce(new Vector3(Mathf.Sign(target.x - transform.position.x), 0f, 0f) * speed);

		// stop charing if I got to the charge target:
		if(charging && Mathf.Abs(transform.position.x - chargeTarget.x) < .4f) {
			charging = false;
		}

		//if HP is low enough, keep charing over and over:
		if (lastHP <= hpToRampage && Mathf.Abs(transform.position.x - homeTarget.x) < .4f) {
			charging = true;
		}

		// flip sprite towards target:
		spriteRenderer.flipX = transform.position.x < target.x;
	}

}

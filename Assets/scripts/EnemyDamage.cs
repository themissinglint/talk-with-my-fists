using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {
	public AudioClip attackSFX, deathSFX;
	public GameObject deathsplosion;
	public int hp = 1;
	public float myKnockback = 10f;
	public bool hurtsPlayer = true;
	private float lastHitTime = 0f;
	private float iFrames = .2f;

	public InteractionToastData KillToast;
	
	[Header("Stat Change On Kill")]
	public PlayerStat StatChanged;
	public float StatChangeAmount;
	
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.layer == 9) { // 9 is player

			Character plrChar = collision.gameObject.GetComponent<Character>();
			if (plrChar.MovementState.CurrentState == CharacterStates.MovementStates.Dashing) {
				Debug.Log("dashing player triggered.");
				if(lastHitTime + iFrames > Time.time) {
					Debug.Log("dashing player triggered but in iFrames.");
					//in iFrames, return.
					return;
				}
				// player hurts me
				AudioSource.PlayClipAtPoint(deathSFX, transform.position);
				hp -= Mathf.RoundToInt(PlayerStatus.DamageDealtByDash);
				Instantiate(deathsplosion, transform.position, Quaternion.identity);

				if (hp <= 0) {
					Debug.Log("dashing player triggered, hp now 0");
					int killCount = PlayerStatus.GiveCreditForKilledEnemy(gameObject);
					if (KillToast != null && InteractionToastDisplay.Instance != null) {
						InteractionToastDisplay.Instance.PopToast(KillToast, gameObject, collectableNumber:killCount);
					}
					PlayerStatus.AddStat(StatChanged, StatChangeAmount);
					Destroy(gameObject);
				} else {
					Debug.Log("dashing player triggered, HP is " + hp);
					Vector3 knockbackVector = new Vector3(Mathf.Sign(transform.position.x - plrChar.transform.position.x), 0.1f, 0f);
					GetComponent<Rigidbody2D>().AddForce(knockbackVector * myKnockback, ForceMode2D.Impulse);
					lastHitTime = Time.time;
				}

			} else if(hurtsPlayer){
				// I hurt player
				AudioSource.PlayClipAtPoint(attackSFX, transform.position);
				plrChar.GetComponent<PlayerDamage>().TakeDamage(gameObject);
			}
		}
	}
}

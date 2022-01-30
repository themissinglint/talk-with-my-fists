using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {
	public AudioClip attackSFX, deathSFX;
	public GameObject deathsplosion;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.layer == 9) { // 9 is player

			Character plrChar = collision.gameObject.GetComponent<Character>();
			if (plrChar.MovementState.CurrentState == CharacterStates.MovementStates.Dashing) {
				// player hurts bee
				AudioSource.PlayClipAtPoint(deathSFX, transform.position);
				Instantiate(deathsplosion, transform.position, Quaternion.identity);
				Destroy(gameObject);

			} else {
				// bee hurts player
				AudioSource.PlayClipAtPoint(attackSFX, transform.position);
				plrChar.GetComponent<PlayerDamage>().TakeDamage(gameObject);
			}
		}
	}
}

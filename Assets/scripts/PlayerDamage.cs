using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
	public float knockback = 40f;
	public float stunTime = 3f;
	public float stunDelay = .2f;
	public Vector2 stunMaxSpeed = new Vector2(1f, 9f);
	public Vector2 stunMaxSpeedFadeOut = new Vector2(4f, 12f);
	private float stunTimer;
	private float flashTimer;
	private bool isRed;
	private CorgiController controller;
	private SpriteRenderer rend;
	private CharacterDash dashScript;
	private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
		controller = GetComponent<CorgiController>();
		rend = GetComponentInChildren<SpriteRenderer>();
		dashScript = GetComponent<CharacterDash>();
	}

	// Update is called once per frame
	void Update()
    {
		if (stunTimer > 0f) {

			flashTimer -= Time.deltaTime;
			if(flashTimer < 0f) {
				flashTimer = .1f;
				if (isRed) {
					rend.color = Color.white;
					isRed = false;
				} else {
					rend.color = Color.red;
					isRed = true;
				}
			}

			stunTimer -= Time.deltaTime;
			if(stunTimer < stunTime - stunDelay) {
				// wait .1 second before imposing max speed so knockback can work.
				Vector2 maxSpeed = Vector2.Lerp(stunMaxSpeed, stunMaxSpeedFadeOut, 1f - stunTimer / (stunTime - stunDelay));
				controller.Parameters.MaxVelocity = maxSpeed;
			}
			if (stunTimer <= 0f) {
				controller.Parameters.MaxVelocity = new Vector2(100f,100f);
				rend.color = Color.white;
				dashScript.enabled = true;
			}
		}
    }

	public void TakeDamage(GameObject hurter) {

		Vector3 knockbackVector = new Vector3(Mathf.Sign(transform.position.x - hurter.transform.position.x), 0f, 0f);
		controller.Parameters.MaxVelocity = new Vector2(100f, 100f);//back to maxk speed so the knockback works.
		controller.SetForce(knockbackVector * knockback);
		stunTimer = stunTime;
		baseColor = rend.color;
		dashScript.enabled = false;
	}
}

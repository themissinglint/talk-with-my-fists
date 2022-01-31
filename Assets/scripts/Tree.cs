using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
	public int woodCount;
	public int acornCount;
	public GameObject wood;
	public GameObject acorn;
	public float explodeImpulse;
	public AudioClip chopSFX;
	public AudioClip landSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void Land() {
		Rigidbody2D rb;
		AudioSource.PlayClipAtPoint(landSFX, transform.position);
		for (int i=0; i < woodCount; i++) {
			rb = Instantiate(wood, transform.position + Vector3.up + transform.up * Random.Range(0f, 8f), Quaternion.AngleAxis(Random.Range(0, 360), Vector3.back)).GetComponent<Rigidbody2D>();
			rb.AddForce(new Vector2(Random.Range(-explodeImpulse, explodeImpulse), Random.Range(-explodeImpulse, explodeImpulse)), ForceMode2D.Impulse);
			rb.AddTorque(Random.Range(-1f, 1f));
		}
		for (int i = 0; i < acornCount; i++) {
			rb = Instantiate(acorn, transform.position + Vector3.up + transform.up * Random.Range(0f, 8f), Quaternion.AngleAxis(Random.Range(0, 360), Vector3.back)).GetComponent<Rigidbody2D>();
			rb.AddForce(new Vector2(Random.Range(-explodeImpulse, explodeImpulse), Random.Range(-explodeImpulse, explodeImpulse)), ForceMode2D.Impulse);
			rb.AddTorque(Random.Range(-1f, 1f));
		}
		Destroy(this);
	}


	void OnTriggerStay2D(Collider2D collider) {
		GameObject ob = collider.gameObject;
		if(ob.layer == 9) {//9 is player layer
			Character character = ob.GetComponent<Character>();
			if(character.MovementState.CurrentState == CharacterStates.MovementStates.Dashing) {
				Debug.Log("Tiiiiiimmber!");
				Animation anim = GetComponent<Animation>();
				AudioSource.PlayClipAtPoint(chopSFX, transform.position);
				anim.Play();
			}
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : MonoBehaviour
{
	public AudioClip pickupSFX;

	private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.layer == 8) { // 8 is platforms
			rb.bodyType = RigidbodyType2D.Static;
			GetComponent<Collider2D>().isTrigger = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.layer == 9) { // 9 is player

			//TODO: give player acron.
			Debug.Log("You get an Acorn!");
			AudioSource.PlayClipAtPoint(pickupSFX, transform.position);
			Destroy(gameObject);
		}
	}
}

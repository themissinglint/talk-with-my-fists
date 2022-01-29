using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
	public float attractionForce = 1f;
	public AudioClip pickupSFX;
	private GameObject player;
	private Rigidbody2D rb;
	// Start is called before the first frame update
	void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
		rb.AddForce((player.transform.position - transform.position).normalized * attractionForce);
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.layer == 9) { // 9 is player

			//TODO: give player acron.
			Debug.Log("You get an Wood!");
			AudioSource.PlayClipAtPoint(pickupSFX, transform.position);
			Destroy(gameObject);
		}
	}
}

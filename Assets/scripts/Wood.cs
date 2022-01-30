using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Collectable {
	
	public float attractionForce = 1f;
	private GameObject player;
	
	protected override int CollectableCount => PlayerStatus.WoodCount;
	
	protected override void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
		rb.AddForce((player.transform.position - transform.position).normalized * attractionForce);
    }

	protected override void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.layer == 9) { // 9 is player
			PlayerStatus.WoodCount++;
			Debug.Log("You get an Wood!");
		}
		base.OnTriggerEnter2D(collision);
	}
}

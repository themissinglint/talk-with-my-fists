using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : Collectable {

	protected override int CollectableCount => PlayerStatus.AcornCount;
	
	protected override void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.layer == 9) { // 9 is player
			Debug.Log("You get an Acorn!");
			PlayerStatus.AcornCount++;
		}
		base.OnTriggerEnter2D(collision);
	}
	
}

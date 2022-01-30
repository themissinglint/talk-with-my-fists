using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Collectable {
    
    protected override int CollectableCount => PlayerStatus.MushroomCount;
    
    protected override void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == 9) { // 9 is player
            Debug.Log("You get a Mushroom!");
            PlayerStatus.MushroomCount++;
        }
        base.OnTriggerEnter2D(collision);
    }
    
}

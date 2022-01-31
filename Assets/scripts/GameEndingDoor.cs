using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndingDoor : MonoBehaviour {

    private bool _playerHasWon = false;
    
    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == 9) { // 9 is player
            if (_playerHasWon) return;
            _playerHasWon = true;
            PlayerStatus.PlayerHasWon = true;
            WorldStatus.TimeThatPlayerWon = Time.time;
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
    
    public InteractionToastData PickupToast;
    public AudioClip pickupSFX;

    [Header("Stat Change")]
    public PlayerStat StatChanged;
    public float StatChangeAmount;

    protected virtual int CollectableCount => 0;
    
    protected Rigidbody2D rb;
    
    // Start is called before the first frame update
    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 8) { // 8 is platforms
            rb.bodyType = RigidbodyType2D.Static;
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == 9) { // 9 is player
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position);
            if (InteractionToastDisplay.Instance != null) {
                InteractionToastDisplay.Instance.PopToast(PickupToast, gameObject, collectableNumber: CollectableCount);
            }
            PlayerStatus.AddStat(StatChanged, StatChangeAmount);
            Destroy(gameObject);
        }
    }
    
}

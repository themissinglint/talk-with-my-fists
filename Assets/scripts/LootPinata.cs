using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPinata : MonoBehaviour
{
	public int count;
	public GameObject loot;
	public float explodeImpulse;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void Explode() {
		Rigidbody2D rb;
		for (int i = 0; i < count; i++) {
			rb = Instantiate(loot, transform.position + Vector3.up, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.back)).GetComponent<Rigidbody2D>();
			rb.AddForce(new Vector2(Random.Range(-explodeImpulse, explodeImpulse), Random.Range(-explodeImpulse, explodeImpulse)), ForceMode2D.Impulse);
			rb.AddTorque(Random.Range(-1f, 1f));
		}
		Destroy(gameObject);
	}
}

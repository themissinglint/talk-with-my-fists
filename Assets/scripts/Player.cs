using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Vector2 speed;
	public float jumpImpulse;
	private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
		Vector2 input = Vector2.zero;
		if (Input.GetKey(KeyCode.LeftArrow)) {
			input.x -= speed.x;
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			input.x += speed.x;
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			input.y += speed.y;
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			input.y -= speed.y;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			rb.AddForce(Vector2.down * jumpImpulse, ForceMode2D.Impulse);
		}

		rb.AddForce(input);
	}
}

using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {

	public float lifetime;
	public bool straightShot;
	public bool playerOneShot;

	void Start()
	{
		Destroy(gameObject, lifetime);
	}

	void OnTriggerEnter2D (Collider2D col) {
		// when cannon balls collide, they both lose momentum and fall into the sea
		if (col.gameObject.CompareTag ("CannonBall") 
			&& (playerOneShot != col.gameObject.GetComponent<CannonBall>().playerOneShot)
			&& (col.gameObject.GetComponent<Torpedo>() == null)) {
			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			rb.velocity = new Vector2(0f,0f);
			rb.gravityScale = 1f;
		}
	}
}

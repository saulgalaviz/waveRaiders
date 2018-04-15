using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : CannonBall {

	public float speed = 1f;
	//public bool playerOneShot;
	public Wave myWave;
	private float stepWidth;

	public GameObject damageSound;
	public GameObject smallExplosion;

	// Use this for initialization
	void Start () {
		myWave = GameObject.Find ("WaveManager").GetComponent<Wave> ();
		stepWidth = (myWave.lastPosition.position.x - myWave.firstPosition.position.x) / myWave.numPoints;
	}
	
	// Update is called once per frame
	void Update () {
		int stepPos = Mathf.RoundToInt( (transform.position.x - myWave.firstPosition.position.x) / stepWidth);
		if (stepPos < 0 || stepPos >= myWave.numPoints) {
			Destroy (gameObject);
			return;
		}
		transform.position = new Vector3 (transform.position.x, myWave.edgeHog.points [stepPos].y, transform.position.z);
		if (playerOneShot) {
			transform.Translate (Vector2.right * speed * Time.deltaTime);
		} else {
			transform.Translate (Vector2.right * -1 * speed * Time.deltaTime);
		}
	}


	void OnTriggerEnter2D (Collider2D col) {
		// if it's a cannon ball, DIE
		if (col.gameObject.CompareTag ("CannonBall") && (playerOneShot != col.gameObject.GetComponent<CannonBall>().playerOneShot)) {
			Instantiate (damageSound, col.gameObject.transform.position, Quaternion.identity);
			Instantiate (smallExplosion, col.gameObject.transform.position, Quaternion.identity);
			//Destroy (col.gameObject);
			Destroy (gameObject);
		}
	}
}

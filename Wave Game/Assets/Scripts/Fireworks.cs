using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour {

	bool mode = false;
	public float speed = 3f;
	public float timer = 2.5f;
	private Vector3 course;
	private ParticleSystem parts;
	private SpriteRenderer sprite;
	private AudioSource aud;

	// Use this for initialization
	void Start () {
		course = new Vector3 (Random.Range (-1f, 1f), 1f, 0f);
		sprite = GetComponent<SpriteRenderer> ();
		parts = GetComponent<ParticleSystem> ();
		aud = GetComponent<AudioSource> ();
		timer -= Random.Range (0f, 0.3f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!mode) {
			if (timer <= 0) {
				sprite.enabled = false;
				parts.Play ();
				aud.Play ();
				mode = true;
			} else {
				timer -= Time.deltaTime;
				transform.Translate (course * speed * Time.deltaTime);
			}
		} else {
			if (!parts.IsAlive ()) {
				Destroy (gameObject);
			}
		}
	}
}

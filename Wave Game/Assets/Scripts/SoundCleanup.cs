using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCleanup : MonoBehaviour {
	
	private AudioSource aud;

	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		if (!aud.isPlaying) {
			Destroy (gameObject);
		}
	}
}

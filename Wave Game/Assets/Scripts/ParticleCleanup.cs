using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCleanup : MonoBehaviour {

	private ParticleSystem parts;

	// Use this for initialization
	void Start () {
		parts = GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update () {
		if (!parts.IsAlive ()) {
			Destroy (gameObject);
		}
	}
}

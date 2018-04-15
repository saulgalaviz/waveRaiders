using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyDrift : MonoBehaviour {

	public bool driftRight = true;
	public float yMove = 0f;
	private float xMove;
	private int directionMod;

	public float lifeTime = 20f;
	public float middleThreshold = 4f;

	// Use this for initialization
	void Start () {
		xMove = Random.Range (0.5f, 3f);
		if (driftRight) {
			directionMod = 1;
		} else {
			directionMod = -1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		yMove += Random.Range (-0.2f, 0.2f) + (middleThreshold - transform.position.y)*0.025f/middleThreshold;
		transform.Translate (xMove * directionMod * Time.deltaTime, yMove*Time.deltaTime, 0f);

		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0f) {
			Destroy(gameObject);
		}
	}
}

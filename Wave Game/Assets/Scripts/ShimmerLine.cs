using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShimmerLine : MonoBehaviour {

	private LineRenderer myLine;
	private float myTimer;
	private float pauseTime, shimmerTime, maxShimmer;
	private float firstLoc, midLoc, lastLoc;
	private int stage;
	private bool oneDirection;

	// Use this for initialization
	void Start () {
		myLine = GetComponent<LineRenderer> ();
		randomizeVars ();
		myTimer = pauseTime;

		//firstLoc = 0.0f; midLoc = 0.01f; lastLoc = 0.02f;
		//oneDirection = true;
		stage = 3;
	}
	
	// Update is called once per frame
	void Update () {

		if (stage == 0) {
			if (oneDirection) {
				lastLoc = Mathf.Min (lastLoc + Time.deltaTime / shimmerTime, 1.0f);
				if (lastLoc == 1.0f) {
					stage = 1;
				}
			} else {
				firstLoc = Mathf.Max (firstLoc - Time.deltaTime / shimmerTime, 0.0f);
				if (firstLoc == 0.0f) {
					stage = 1;
				}
			}
		} else if (stage == 1) {
			if (oneDirection) {
				midLoc = Mathf.Min (midLoc + Time.deltaTime / shimmerTime, 0.99f);
				if (midLoc == 0.99f) {
					stage = 2;
				}
			} else {
				midLoc = Mathf.Max (midLoc - Time.deltaTime / shimmerTime, 0.01f);
				if (midLoc == 0.01f) {
					stage = 2;
				}
			}
		} else if (stage == 2) {
			if (oneDirection) {
				firstLoc = Mathf.Min (firstLoc + Time.deltaTime / shimmerTime, 0.98f);
				if (firstLoc == 0.98f) {
					stage = 3;
				}
			} else {
				lastLoc = Mathf.Max (lastLoc - Time.deltaTime / shimmerTime, 0.02f);
				if (lastLoc == 0.02f) {
					stage = 3;
				}
			}
		} else if (stage == 3) {
			if (myTimer > 0) {
				myTimer -= Time.deltaTime;
			} else {
				myTimer = pauseTime;
				stage = 0;
				if (randomizeVars()) {
					firstLoc = 0.0f;
					midLoc = 0.01f;
					lastLoc = 0.02f;
				} else {
					firstLoc = 0.98f;
					midLoc = 0.99f;
					lastLoc = 1.0f;
				}
			}
		}

		GradientAlphaKey[] alphaFive = new GradientAlphaKey[] {
			new GradientAlphaKey (0.0f, firstLoc),
			new GradientAlphaKey (maxShimmer, midLoc),
			new GradientAlphaKey (0.0f, lastLoc)
		};
		Gradient grad = new Gradient ();
		grad.SetKeys (myLine.colorGradient.colorKeys, alphaFive);
		myLine.colorGradient = grad;
	}

	private bool randomizeVars () {
		pauseTime = Random.Range (1.0f, 5.0f);
		shimmerTime = Random.Range (0.5f, 3f);
		maxShimmer = Random.Range (0.2f, 1.0f);
		oneDirection = (Random.value >= 0.5f);
		return oneDirection;
	}
}

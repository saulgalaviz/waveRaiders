using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyEvents : MonoBehaviour {

	public float stormLength;
	public float stormTimer;
	public float stormVariance;
	private float myTimer;
	public bool stormOn;

	private WindZone myWind;
	private ParticleSystem skyParts1;
	private ParticleSystem skyParts2;
	private Wave myWave;

	private float partEmission;
	private float windStrength;

	public float randomTimer = 2f;
	public float randomVariance = 5f;
	private float spawnTimer;

	public GameObject[] randomSpawn;

	// Use this for initialization
	void Start () {
		myWind = GetComponentInChildren<WindZone> ();
		skyParts1 = transform.GetChild (1).GetComponent<ParticleSystem> ();
		skyParts2 = transform.GetChild (2).GetComponent<ParticleSystem> ();
		myWave = GameObject.Find ("WaveManager").GetComponent<Wave> ();
		//stormOn = false;

		myTimer = stormTimer + Random.Range (0f, stormVariance);
		partEmission = skyParts1.emission.rateOverTimeMultiplier;
		windStrength = myWind.windMain;

		if (stormOn) {
			myTimer = stormLength;

			ParticleSystem.EmissionModule emitter = skyParts1.emission;
			emitter.rateOverTimeMultiplier = partEmission * 25;
			emitter = skyParts2.emission;
			emitter.rateOverTimeMultiplier = partEmission * 25;
			myWind.windMain = windStrength * 4;

			myWave.AddWave (0f, 2f, 3f, 20f);
			myWave.AddWave (38f, 1.2f, -4.5f, 15f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (myTimer > 0) {
			myTimer -= Time.deltaTime;
		} else {
			if (!stormOn) {
				myTimer = stormLength;
				stormOn = true;
                gameObject.GetComponent<AudioSource>().Play();

				ParticleSystem.EmissionModule emitter = skyParts1.emission;
				emitter.rateOverTimeMultiplier = partEmission * 25;
				emitter = skyParts2.emission;
				emitter.rateOverTimeMultiplier = partEmission * 25;
				myWind.windMain = windStrength * 4;

				myWave.AddWave (0f, 2f, 3f, 20f);
				myWave.AddWave (38f, 1.2f, -4.5f, 15f);
			} else {
				myTimer = stormTimer + Random.Range (0f, stormVariance);
				stormOn = false;

				ParticleSystem.EmissionModule emitter = skyParts1.emission;
				emitter.rateOverTimeMultiplier = partEmission;
				emitter = skyParts2.emission;
				emitter.rateOverTimeMultiplier = partEmission;
				myWind.windMain = windStrength;
			}
		}


		if (spawnTimer > 0) {
			spawnTimer -= Time.deltaTime;
		} else {
			spawnTimer = randomTimer + Random.Range (0f, randomVariance);
			GameObject newObject = randomSpawn [Random.Range (0,randomSpawn.Length)];
			RotateFall rotFall = newObject.GetComponent<RotateFall> ();
			LazyDrift lazDrift = newObject.GetComponent<LazyDrift> ();
			if (rotFall != null) {
				if (Random.Range (0f, 1f) > 0.5f) {
					Instantiate (newObject, new Vector3 (Random.Range (-10f, 10f), Random.Range (11f, 13f), 0f), Quaternion.identity);
				} else {
					GameObject instantiatedObject = (GameObject)Instantiate (newObject, new Vector3 (Random.Range (-10f, 10f), -2f), Quaternion.identity);
					instantiatedObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(Random.Range(-150f,150f), Random.Range(300f,350f)));
				}
			} else if (lazDrift != null) {
				if (Random.Range (0f, 1f) > 0.5f) {
					GameObject instantiatedObject = (GameObject)Instantiate (newObject, new Vector3 (13f, Random.Range (0f, 9f), 0f), Quaternion.identity);
					instantiatedObject.GetComponent<LazyDrift> ().driftRight = false;
				} else {
					GameObject instantiatedObject = (GameObject)Instantiate (newObject, new Vector3 (-13f, Random.Range (0f, 9f), 0f), Quaternion.identity);
					instantiatedObject.GetComponent<LazyDrift> ().driftRight = true;
				}
			}
		}
	}
}

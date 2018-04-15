using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour {

	public bool playerOne;
	private int facing = 1;

	public int health;
	public int maxHealth = 10;

	public GameObject cannonSound;
	public GameObject torpedoSound;
	public GameObject damageSound;
	public GameObject explodeSound;

	public GameObject smallExplosion;
	public GameObject explosion;
	public GameObject smoke;
	public GameObject projectile;
	public GameObject projectile2;
	public GameObject projectile3;
	public Transform cannonPoint1;
	public Transform cannonPoint2;

	public GameObject fireSlider;
	private Slider mySlider;
	private Image myFilling;

	public GameObject healthBar;

	public float moveSpeed = 1.5f;
	public float cannonForce = 500;
	public float torqueMod = 100;
	//public float cannonSpeed;

	public float firingDelay = 1f;
	private float firingTimer;

	public int currStage = 1;


	// Use this for initialization
	void Start () {
		// set your current health TO THE MAX, and switch your firing delay to READY
		health = maxHealth;
		firingTimer = 0;

		// if you're player two, everything is BACKWARDS, take that into account
		if (playerOne) {
			facing = 1;
		} else {
			facing = -1;
		}

		// retrieve your slider
		if (fireSlider != null) {
			mySlider = fireSlider.GetComponent<Slider> ();
			myFilling = fireSlider.transform.GetChild (1).GetChild (0).GetComponent<Image> ();
		}

		switch (currStage) {
		case 1:
			GetComponent<Animator> ().Play ("Ship");
			break;
		case 2:
			GetComponent<Animator> ().Play ("Blimp");
			break;
		case 3:
			GetComponent<Animator> ().Play ("Space");
			break;
		default:
			print ("Argh we don't know what stage it is how do we animate?!");
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {

		//print (transform.localEulerAngles);
		if (transform.localEulerAngles.z > 80f && transform.localEulerAngles.z < 180f) {
			transform.localEulerAngles = new Vector3 (transform.rotation.x, transform.rotation.y, 80f);
		} else if (transform.localEulerAngles.z < 280f && transform.localEulerAngles.z > 180f) {
			//print ("Before: " + transform.rotation);
			transform.localEulerAngles = new Vector3 (transform.rotation.x, transform.rotation.y, 280f);
			//print ("  After: " + transform.rotation);
		}

		// check the firing timer to see if the boat is ready to raise hell
		if (firingTimer > 0) {
			firingTimer -= Time.deltaTime;
		} else {
			// when the firing buttons are pressed, FIRE THE CANNONS! (controls are player-dependent)
			if ((playerOne && Input.GetKeyDown (KeyCode.S)) || 
				(!playerOne && Input.GetKeyDown(KeyCode.UpArrow))) {
				// fire a cannon ball straight out the front of the ship
				firingTimer = firingDelay;
				GameObject instantiatedProjectile = (GameObject)Instantiate (projectile, cannonPoint1.position, Quaternion.identity);
				instantiatedProjectile.GetComponent<Rigidbody2D> ().AddForce (transform.right * cannonForce * facing);
				//GetComponent<Rigidbody2D> ().AddTorque (torqueMod);
				instantiatedProjectile.GetComponent<CannonBall> ().playerOneShot = playerOne;
				GameObject instantiatedSmoke = (GameObject)Instantiate (smoke, new Vector3(cannonPoint1.position.x + 0.1f*facing, cannonPoint1.position.y), Quaternion.identity);
				instantiatedSmoke.transform.SetParent (gameObject.transform, true);
				Instantiate (cannonSound, cannonPoint1.position, Quaternion.identity);
			}
			if ((playerOne && Input.GetKeyDown (KeyCode.W)) || 
				(!playerOne && Input.GetKeyDown(KeyCode.Return))) {
				// fire a cannon ball in an up-up-forward direction (that's a technical term)
				firingTimer = firingDelay;
				GameObject instantiatedProjectile = (GameObject)Instantiate (projectile2, cannonPoint2.position, Quaternion.identity);
				instantiatedProjectile.GetComponent<Rigidbody2D> ().AddForce (new Vector3(2*transform.up.x+transform.right.x*facing, 2*transform.up.y+transform.right.y).normalized * cannonForce);
				instantiatedProjectile.GetComponent<CannonBall> ().playerOneShot = playerOne;
				GameObject instantiatedSmoke = (GameObject)Instantiate (smoke, new Vector3(cannonPoint2.position.x + 0f*facing, cannonPoint2.position.y), Quaternion.identity);
				instantiatedSmoke.transform.SetParent (gameObject.transform, true);
				Instantiate (cannonSound, cannonPoint2.position, Quaternion.identity);
			}
			if ((playerOne && Input.GetKeyDown (KeyCode.X)) || 
				(!playerOne && Input.GetKeyDown(KeyCode.DownArrow))) {
				// FIRE TORPEDO!
				firingTimer = firingDelay*1.2f;
				GameObject instantiatedProjectile = (GameObject)Instantiate (projectile3, cannonPoint1.position, Quaternion.identity);
				instantiatedProjectile.GetComponent<Torpedo> ().playerOneShot = playerOne;
				Instantiate (torpedoSound, cannonPoint1.position, Quaternion.identity);
			}
		}

		// update the firing delay's UI slider
		if (fireSlider != null) {
			mySlider.value = (firingDelay - firingTimer) / firingDelay;
			if (firingTimer <= 0) {
				myFilling.color = new Color (75 / 255, 255 / 255, 41 / 255);
			} else {
				myFilling.color = new Color (255 / 255, 81 / 255, 81 / 255);
			}
		}

		// this was FORMERLY the movement code, preserved for posterity
		/*float translation = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
		transform.Translate (Vector2.right * translation * facing);*/
	}


	// collision code touching the wave (...might not be relevant anymore)
	void OnCollisionStay2D (Collision2D collision) {
		if (collision.gameObject.CompareTag ("Wave")) {
			if (transform.localEulerAngles.z > 45f && transform.localEulerAngles.z < 180f) {
				GetComponent<Rigidbody2D> ().AddTorque (-torqueMod);
			} else if (transform.localEulerAngles.z < 315f && transform.localEulerAngles.z > 180f) {
				GetComponent<Rigidbody2D> ().AddTorque (torqueMod);
			}
		}
		/*if (collision.gameObject.CompareTag ("CannonBall") && 
			(collision.gameObject.GetComponent<CannonBall>().playerOneShot != playerOne)) {
			Destroy (collision.gameObject);
			healthBar.transform.GetChild (health - 1).gameObject.SetActive (false);
			if (--health <= 0) {
				Sink ();
			}
		}*/
	}

	// collision code for trigger-based cannon balls
	void OnTriggerEnter2D (Collider2D col) {
		// if it's a cannon ball, destroy it and damage the ship
		if (col.gameObject.CompareTag ("CannonBall") &&
		    (col.gameObject.GetComponent<CannonBall> ().playerOneShot != playerOne)) {
			Instantiate (damageSound, col.gameObject.transform.position, Quaternion.identity);
			Instantiate (smallExplosion, col.gameObject.transform.position, Quaternion.identity);
			Destroy (col.gameObject);
			healthBar.transform.GetChild (health - 1).gameObject.SetActive (false);
			if (--health <= 0) {
				// if the ship runs out of health, SINK IT
				Sink ();
			}
		}
	}

	// blow up the ship when it dies, adding in a fiery death explosion
	public void Sink () {
		Instantiate (explosion, transform.position, Quaternion.identity);
		Instantiate (explodeSound, transform.position, Quaternion.identity);
		GameObject.Find ("VictoryManager").GetComponent<VictoryManager> ().DeclareVictory (!playerOne);
		Destroy (gameObject);
	}
}

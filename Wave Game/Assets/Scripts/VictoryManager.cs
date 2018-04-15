using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class VictoryManager : MonoBehaviour {

	private bool gameOver = false;
	public float doneTimer = 10f;
	public float partyTimer = 0.5f;
	private float countdown;
	public GameObject victoryText;
	public GameObject firework;

	// Use this for initialization
	void Start () {
		countdown = partyTimer;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver) {
			if (countdown > 0) {
				countdown -= Time.deltaTime;
			} else {
				countdown = partyTimer + Random.Range (0f, 2f);
				Instantiate (firework, new Vector3(Random.Range(-5f,5f), Random.Range(-1f,0f), 0), Quaternion.identity);
			}
			if (doneTimer > 0) {
				doneTimer -= Time.deltaTime;
			} else {
                // return to the main menu!  ...not implemented yet, but we can restart this scene
                if (SceneManager.GetActiveScene().buildIndex < 3)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                else
                    SceneManager.LoadScene(0);
            }
		}
	}

	public void DeclareVictory (bool playerOne) {
		gameOver = true;
		victoryText.SetActive (true);
		if (playerOne) {
			victoryText.GetComponent<Text> ().text = "CONGRATULATIONS!\nPLAYER 1 WINS!";
		} else {
			victoryText.GetComponent<Text> ().text = "CONGRATULATIONS!\nPLAYER 2 WINS!";
		}
	}
}

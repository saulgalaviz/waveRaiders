using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	public Text titleText;
	public Text battleText;
	public Text helpText;
	public Text creditsText;
	public Text quitText;
	public GameObject battleButton;
	public GameObject helpButton;
	public GameObject creditsButton;
	public GameObject quitButton;
	private bool titleMode;

	// Use this for initialization
	void Start () {
		titleMode = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (titleMode && Input.anyKeyDown) {
			titleMode = false;
			StartCoroutine (TitleTransition (1.0f));
		}
	}


	public void battleButtonPress ()
	{
		Debug.Log("Battle");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void helpButtonPress ()
	{

	}

	public void creditsButtonPress ()
	{

	}

	public void quitButtonPress ()
	{
		Application.Quit ();
	}


	public IEnumerator TitleTransition(float flashTime)
	{
		float timer = flashTime;
		Color textColor = titleText.color;

		while (timer > 0) {
			textColor.a = (timer / flashTime);
			titleText.color = textColor;

			timer -= Time.fixedDeltaTime;
			yield return new WaitForSeconds (Time.fixedDeltaTime);
		}

		timer = flashTime;
		titleText.gameObject.SetActive (false);
		battleButton.SetActive (true);
		creditsButton.SetActive (true);
		helpButton.SetActive (true);
		quitButton.SetActive (true);

		while (timer > 0) {
			textColor.a = 1- (timer / flashTime);
			battleText.color = creditsText.color = helpText.color = quitText.color = textColor;

			timer -= Time.fixedDeltaTime;
			yield return new WaitForSeconds (Time.fixedDeltaTime);
		}
	}
}
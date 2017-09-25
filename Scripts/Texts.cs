using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Texts : MonoBehaviour {

	public static Texts ins;

	public Text scoreText;
	public Text timerText;

	public GameObject gameOverPanel;
	public Text yourScoreTxt;
	public Text highScoreTxt;

	private int score, time;

	// instance, so I can use functions in Texts
	private static Texts _instance;
	public static Texts instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<Texts>();
			}
			return _instance;
		}
	}


	void Awake() {
		ins = GetComponent<Texts>();

		//initiate
		score = 0;
		time = 60;
	}

	// Use this for initialization
	void Start () {
		
		StartCoroutine (Clock ()); //Start countdown
	}

	IEnumerator Clock(){// countdown function
		while (time > 0) {
			yield return new WaitForSeconds (1);
			time = time - 1;
			timerText.text = time.ToString ();
		}
		if (time <= 0) {
			time = 0;
			StartCoroutine(WaitForEnd());
		}
	}

	public void IncreaseScore(int value){ // increase the score
		score += value;
		scoreText.text = score.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator WaitForEnd() {
		yield return new WaitForSeconds(.25f);
		GameOver();
	}

	public void GameOver() {

		gameOverPanel.SetActive(true);// show the gameover panel

		if (score > PlayerPrefs.GetInt("HighScore")) { // Save and change the highScore
			PlayerPrefs.SetInt("HighScore", score);
			highScoreTxt.text = "New Best: " + PlayerPrefs.GetInt("HighScore").ToString();
		} else {
			highScoreTxt.text = "Best: " + PlayerPrefs.GetInt("HighScore").ToString();
		}

		yourScoreTxt.text = score.ToString();
	}
}

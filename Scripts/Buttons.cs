using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

	public static GameManager instance;

	public GameObject faderObj;
	public Image faderImg;
	public float fadeSpeed = .02f;

	private Color fadeTransparency = new Color(0, 0, 0, .04f);
	private string currentScene;
	private AsyncOperation async;

	public void LoadScene(){
		SceneManager.LoadScene ("Gaming");
		
	}

	public void ReturnToTitle(){
		SceneManager.LoadScene ("Title");

	}

	public void ExitGame() {
		// If we are running in a standalone build of the game
		#if UNITY_STANDALONE
		// Quit the application
		Application.Quit();
		#endif

		// If we are running in the editor
		#if UNITY_EDITOR
		// Stop playing the scene
		UnityEditor.EditorApplication.isPlaying = false;
		#endif

		PlayerPrefs.DeleteAll ();
	}



}

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public SpawnController spawnController;

	public fade fadeScript;
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameoverText;
	public float fadeTime;
	public float musicVolume;
	public bool gameover;
	// private 
	private int score;
	private bool restart;

	public void AddScore(int newScore)
	{
		score += newScore;
		UpdateScore ();
	}
	void UpdateScore()
	{
		if (scoreText != null)
			scoreText.text = "Score: " + score;
	} 
	void Start()
	{
		gameover = false;
		restart = false;
		if (restartText != null)
		{
			restartText.text = "";
			gameoverText.text = "";
		}
		score = 0;

	}
	void Awake()
	{
		GameObject spawnControllerObj = GameObject.FindWithTag ("SpawnController");
		if (spawnControllerObj != null) 
			spawnController = spawnControllerObj.GetComponent<SpawnController> ();
		else 
			Debug.Log ("Error: Game controller cannot find spawnController!");
		UpdateScore ();
		StartCoroutine (GameLoop ());
	}
	public void GameOver()
	{
		gameoverText.text = "Game Over!";
		gameover = true;
		GetComponent<AudioSource> ().volume = 0;

	}
	void Update()
	{
		if (gameover && Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
	// MAIN LOOP
	IEnumerator GameLoop()
	{
		yield return new WaitForSeconds(0.5f);
		Debug.Log ("Game running");
		/*
		 * GAME START Fade in
		fade fadeInst = Instantiate (fadeScript) as fade;
		fadeInst.FadeIn (fadeTime);
		yield return new WaitForSeconds (fadeTime);
		GetComponent<AudioSource> ().volume = musicVolume;
		*/
		gameover = false;
		while (!gameover) 
		{
			spawnController.KickWave(5 /*num to spawn in wave*/ ,4 /*color type count*/,3/*move speed*/, 2 /*spawn speed*/);
			const float sleepTime = 0.5f;
			while (spawnController.hazardCount>0)
			{
				yield return new WaitForSeconds(sleepTime);
				if (gameover)
					break;
			}
		}
		/*
		 * 	GAME OVER - Fade Out

		fadeInst = Instantiate (fadeScript) as fade;
		fadeInst.FadeOut (fadeTime);
		yield return new WaitForSeconds (fadeTime);
		restartText.text = "Press 'R' to Restart";
		*/
	}
}

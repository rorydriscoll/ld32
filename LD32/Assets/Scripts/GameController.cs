using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public SpawnController spawnController;
	public GameObject screenFader;
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameoverText;
	public float fadeTime;
	public float waitBetweenWaves;

	// private 
	private int score;
	private int curWave;
	enum GameState
	{
		kStartGame,
		kPlaying,
		kGameOver,
		kFadeOut,
	};
	GameState curState;

	public void SetGameOver()
	{
		SetGameState(GameState.kGameOver);
		if (gameoverText!=null)
			gameoverText.text = "Game Over!";
		if (restartText != null)
			restartText.text = "Press 'R' to Restart";
		if (GetComponent<AudioSource> () != null)
			GetComponent<AudioSource> ().volume = 0;
	}
	public bool IsGameOver()
	{
		return curState == GameState.kGameOver;
	}
	public void AddScore(int newScore)
	{
		score += newScore;
		UpdateScore();
	}
	void SetGameState(GameState state)
	{
		curState= state;
	}
	void UpdateScore()
	{
		if (scoreText != null)
			scoreText.text = "Score: " + score;
	} 
	void Start()
	{
		InitGame ();
	}
	void Awake()
	{
		GameObject spawnControllerObj = GameObject.FindWithTag ("SpawnController");
		if (spawnControllerObj != null) 
			spawnController = spawnControllerObj.GetComponent<SpawnController> ();
		else 
			Debug.Log ("Error: Game controller cannot find spawnController!");
		StartCoroutine (GameLoop ());
	}
	void Update()
	{
		if (curState==GameState.kGameOver && Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
	void InitGame()
	{
		SetGameState(GameState.kStartGame);
		curWave = -1;
		score = 0;
		if (restartText != null)
			restartText.text = "";
		if (gameoverText != null)
			gameoverText.text = "";
		UpdateScore ();

	}

	// MAIN LOOP
	IEnumerator GameLoop()
	{
		Debug.Log ("Game Running");
		for(;;)
		{
			switch (curState)
			{
				case GameState.kStartGame:
					//yield return FadeIn();
					GameObject fader = Instantiate (screenFader) as GameObject;
					Debug.Log ("fader = " + fader);
					fader.GetComponent<fade>().FadeIn(fadeTime);
					yield return new WaitForSeconds (fadeTime);
					InitGame ();
					SetGameState(GameState.kPlaying);
					break;
				case GameState.kPlaying:
					yield return new WaitForSeconds (waitBetweenWaves);
					++curWave;
					spawnController.KickWave(curWave);
					const float sleepTime = 0.25f;
					// while wave is in progress
					while (spawnController.hazardCount>0)
					{
						yield return new WaitForSeconds(sleepTime);
						if (curState == GameState.kGameOver)
							break; // abort wave
					}
					break;
				case GameState.kGameOver:
					break;
				case GameState.kFadeOut:
					fader = Instantiate (screenFader) as GameObject;
					fader.GetComponent<fade>().FadeOut(fadeTime);
					yield return new WaitForSeconds (fadeTime+0.01f);
					SetGameState(GameState.kStartGame);
					break;
			}
		}
	}
}




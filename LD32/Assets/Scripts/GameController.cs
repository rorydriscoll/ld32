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
	public int pointsPerKill = 1;
	public int Override_WaveID = -1;
	
	// private 
	private int score;
	private int enemyGoalCount;
	private int curWave;
	enum GameState
	{
		kFadeIn,
		kStartGame,
		kKickWave,
		kWait,
		kGameOver,
		kFadeOut,
	};
	GameState curState = GameState.kWait, lastState;
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
	public void SetKickWave()
	{
		if (curState != GameState.kGameOver )
			SetGameState(GameState.kKickWave);
	}
	public void FadeDone()
	{
		if (lastState == GameState.kFadeIn)
			SetGameState(GameState.kStartGame);
		else
			RestartGame();
	}
	public bool IsGameOver()
	{
		return curState == GameState.kGameOver;
	}
	public void AddScore(int pts)
	{
		score += pts;
		UpdateScore();
	}
	public void EnemyReachedGoal()
	{
		++enemyGoalCount;
	}
	public void EnemyKilled(Identifier id /* who was killed*/)
	{
		score += pointsPerKill;
		UpdateScore ();
	}
	void PrintState(string prefix, GameState state)
	{
		Debug.Log (prefix + state);
	}
	void SetGameState(GameState state)
	{
		PrintState("Switching to game state ", state);
		PrintState("Current game state is ", curState);
		lastState = curState;
		curState = state;
	}
	void UpdateScore()
	{
		if (scoreText != null)
			scoreText.text = "Score: " + score;
	} 
	void Start()
	{
	}
	void RestartGame()
	{
		// we should be faded out to black
		// clean up and fade in
		Debug.Log ("Print restarting game");
		SetGameState(GameState.kFadeIn);
	}
	void Awake()
	{
		GameObject spawnControllerObj = GameObject.FindWithTag ("SpawnController");
		if (spawnControllerObj != null) 
			spawnController = spawnControllerObj.GetComponent<SpawnController> ();
		else 
			Debug.Log ("Error: Game controller cannot find spawnController!");
		Debug.Log ("Game Controller is AWAKE");
		SetGameState(GameState.kFadeIn);
		StartCoroutine (GameLoop ());
	}
	void Update()
	{
		if (curState==GameState.kGameOver && Input.GetKeyDown(KeyCode.R))
			SetGameState(GameState.kFadeOut);
	}
	void InitGame()
	{
		curWave = -1;
		score = 0;
		enemyGoalCount = 0;
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
				case GameState.kWait:
					break;
				case GameState.kFadeIn:
					GameObject fader = Instantiate (screenFader) as GameObject;
					fader.GetComponent<fade>().FadeIn(fadeTime);
					fader.transform.parent = transform;
					SetGameState(GameState.kWait);
					break;
				case GameState.kStartGame:
					InitGame ();
					SetGameState(GameState.kKickWave);
					break;
				case GameState.kKickWave:
					if (!spawnController.isActive)
					{
						if (spawnController.hazardCount != 0)
							yield return new WaitForSeconds (waitBetweenWaves);
						++curWave;
						if ( Override_WaveID > -1)
							curWave = Override_WaveID;
						spawnController.KickWave(curWave);
						SetGameState(GameState.kWait);
					}
				    break;
				case GameState.kGameOver:
					break;
				case GameState.kFadeOut:
					fader = Instantiate (screenFader) as GameObject;
					fader.GetComponent<fade>().FadeOut(fadeTime);
					fader.transform.parent = transform;
					SetGameState(GameState.kWait);
					break;
			}
			const float sleepTime = 0.15f;
			yield return new WaitForSeconds(sleepTime);
		}
	}
}
using UnityEngine;
using System.Collections;
/*
 * 	NOTE Enemy Goal pos, and offscreen pos is set on a property of the EnemyBehavior script
 */
public class GameController : MonoBehaviour {

	public SpawnController enemySpawner;
	public GameObject screenFader;
	public float fadeTime;
	public float waitBetweenWaves;
	public float timeForOnePuff = 0.15f;
	public int pointsPerKill = 1;
	public int enemyGoalMax = 1;
	public int Override_WaveID = -1;
	GameObject[] puffySmokes;
	// private 
	private int score;
	private int enemyGoalCount;
	private int curWave;
	private float smokeTimeRemain = 0;
	enum GameState
	{
		kFadeIn,
		kStartGame,
		kKickWave,
		kWait,
		kGameOver,
		kFadeOut,
		kReset,
	};
	GameState curState = GameState.kWait, lastState;
	public void SetGameOver()
	{
		if (curState != GameState.kGameOver)
		{
			Debug.Log ("----- GAME OVER ----");
			SetGameState(GameState.kGameOver);
			EnableGameOverText(true);
			if (GetComponent<AudioSource> () != null)
				GetComponent<AudioSource> ().volume = 0;
		}
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
		else if (curState != GameState.kReset)
			RestartGame();
	}
	public bool IsGameOver()
	{
		return curState == GameState.kGameOver;
	}
	public bool IsReseting()
	{
		return curState == GameState.kReset;
	}
	public bool CanFireWeapon()
	{
		Debug.Log ("Spawn COUNT = " + enemySpawner.spawnCount);
		return !IsReseting () && !IsGameOver () && enemySpawner.spawnCount > 0;
	}
	public void AddScore(int pts)
	{
		score += pts;
		UpdateScore();
	}
	public void EnemyReachedGoal()
	{
		smokeTimeRemain += timeForOnePuff;
		if ( ++enemyGoalCount > enemyGoalMax )
			SetGameOver();
	}
	public void EnemyKilled(Identifier id /* who was killed*/)
	{
		score += pointsPerKill;
		UpdateScore ();
	}
	void EnableGameOverText(bool enabled)
	{
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("GameOverText");
		foreach (GameObject go in gos)
			go.GetComponent<GUIText>().enabled = enabled; 
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
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("ScoreText");
		foreach (GameObject go in gos)
			go.GetComponent<GUIText>().text = "Score: " + score;
	} 
	void Start()
	{
	}
	void RestartGame()
	{
		// we should be faded out to black
		// clean up and fade in
		Debug.Log ("Restarting game");
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject go in gos) 
			DestroyObject(go);
		enemySpawner.spawnCount = 0;
		SetGameState(GameState.kReset);
	}
	void Awake()
	{
		Debug.Log ("Game Controller is AWAKE");
		screenFader.transform.parent = transform;
		GameObject spawnControllerObj = GameObject.FindWithTag ("SpawnController");
		if (spawnControllerObj != null) 
			enemySpawner = spawnControllerObj.GetComponent<SpawnController> ();
		else 
			Debug.Log ("Error: Game controller cannot find spawnController!");

		puffySmokes = GameObject.FindGameObjectsWithTag("PuffySmoke");
		if (puffySmokes.Length==0) 
			Debug.Log ("Error: Game controller cannot find it's puffy smoke!");
		else
			StartCoroutine (MakePuffySmoke());
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
		smokeTimeRemain = 0.0f;
		UpdateScore ();
	}
	IEnumerator MakePuffySmoke()
	{
		if (timeForOnePuff <= 0f)
			timeForOnePuff = 0.1f;
		int numEmitters = puffySmokes.Length;
		for (;;)
		{
			if (smokeTimeRemain >= timeForOnePuff)
			{
				for (int i=0;i<numEmitters;i++)
					puffySmokes[i].GetComponent<EllipsoidParticleEmitter>().emit = true;
				smokeTimeRemain -= timeForOnePuff;
			}
			yield return new WaitForSeconds(timeForOnePuff);
			for (int i=0;i<numEmitters;i++)
				puffySmokes[i].GetComponent<EllipsoidParticleEmitter>().emit = false;
		}
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
					EnableGameOverText(false);
					screenFader.GetComponent<fade>().FadeIn(fadeTime);
					SetGameState(GameState.kWait);
					break;
				case GameState.kStartGame:
					InitGame ();
					SetGameState(GameState.kKickWave);
					break;
				case GameState.kKickWave:
					if (!enemySpawner.isActive)
					{
						if (enemySpawner.spawnCount != 0)
							yield return new WaitForSeconds (waitBetweenWaves);
						++curWave;
						if ( Override_WaveID > -1)
							curWave = Override_WaveID;
						enemySpawner.KickWave(curWave);
						SetGameState(GameState.kWait);
					}
				    break;
				case GameState.kGameOver:
					smokeTimeRemain = timeForOnePuff;
					break;
				case GameState.kFadeOut:
					EnableGameOverText(false);
					smokeTimeRemain = 0f;
					screenFader.GetComponent<fade>().FadeOut(fadeTime);
					SetGameState(GameState.kWait);
					break;
				case GameState.kReset:
					yield return new WaitForSeconds(3.0f); // wait for smoke to clear
					SetGameState (GameState.kFadeIn);
					break;
			}
			const float sleepTime = 0.15f;
			yield return new WaitForSeconds(sleepTime);
		}
	}
}
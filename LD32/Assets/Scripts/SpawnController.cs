using UnityEngine;
using System.Collections;


public class SpawnController : MonoBehaviour {

	public AnimationCurve hazardCountCurve;
	public AnimationCurve spawnSpeedCurve;
	public AnimationCurve moveSpeedCurve;
	public AnimationCurve typeCountCurve;
	public GameObject hazard;
	public GameObject playerGO;
	public Color[] colors = new Color[8];
	public bool initColors = true;
	public Vector3 SpawnPos;
	public  int hazardCount;
	public int currentWaveID = 1;
	// private
	private int remainToSpawn ;
	private int numHazardTypes; 
	private GameController gameController;
	private int maxTypes = 6;
	private float speed;
	private float spawnSpeed_;

	// Use this for initialization
	void Start () 
	{
		GameObject gameControllerObj = GameObject.FindWithTag ("GameController");
		if (gameControllerObj != null) 
			gameController = gameControllerObj.GetComponent<GameController> ();
		else 
			Debug.Log ("Spawn controller cannot find GameController!");

		playerGO = GameObject.FindWithTag("Player");
		if (playerGO == null)
			Debug.Log ("ERROR could not find player game object");

		if (initColors)
		{
			colors[0]= Color.black;
			colors[1] = Color.red;
			colors[2] = Color.green;
			colors[3] = Color.blue;
			colors[4] = new Color(255,255,0);
			colors[5] = new Color(255,0,255);
			//colors[6] = new Color(0,255,255);
			//colors[7] = new Color(255,255,255);
		}
	}
	Color PickType()
	{
		/*
		 * 	XXX - Temporary WIP
	 	 */
		int colorIndex = Random.Range(0,numHazardTypes);
		return colors[colorIndex];
	}
	GameObject Player() { return playerGO; }
	void SpawnHazard(GameObject obj)
	{
		Vector3 spawnPos = new Vector3 (Random.Range (-SpawnPos.x, SpawnPos.x), SpawnPos.y, SpawnPos.z);
		GameObject enemyObject = Instantiate (obj, spawnPos, Quaternion.identity) as GameObject;
		enemyObject.GetComponent<EnemyBehavior>().SetTypeSpeedAndController(PickType(), speed,  this);
		Debug.Log ("enemyObject Y = " + enemyObject.transform.position);
		--remainToSpawn;
		++hazardCount;
		Debug.Log ("Spawn enemey count=" + hazardCount);
	}
	// Update is called once per frame
	void Update () {
	
	}
	// count to spawn in the wave and the number of enemey types
	public void KickWave(int count, int numTypes, float moveSpeed, float spawnSpeed)
	{
		if (numTypes > maxTypes)
			numTypes = maxTypes;
		hazardCount = 0;
		remainToSpawn = count;
		numHazardTypes = numTypes;
		speed = moveSpeed;
		spawnSpeed_ = spawnSpeed;
		StartCoroutine (SpawnMain ());
	}
	public void KickWave(int waveID)
	{
		currentWaveID = waveID;
		int numHazards = (int)hazardCountCurve.Evaluate(waveID);
		int numTypes = (int)typeCountCurve.Evaluate(waveID);
		float speed = moveSpeedCurve.Evaluate(waveID);
		float spawnSpeed = spawnSpeedCurve.Evaluate(waveID);
		Debug.Log ("Kick Wave #" + waveID + " Enemies= " + numHazards + " numTypes=" + numTypes + " speed = " + speed + " spawnSpeed = " + spawnSpeed);

		KickWave(numHazards,numTypes, speed, spawnSpeed);
	}
	public void SetPlayerDead()
	{
		if (!gameController.IsGameOver())
			gameController.SetGameOver();
	}
	public bool IsPlayerDead()
	{
		return gameController.IsGameOver();
	}
	IEnumerator SpawnMain()
	{
		Debug.Log ("Spawne started");
		while (!gameController.IsGameOver() && remainToSpawn > 0) 
		{
			SpawnHazard(hazard);
			yield return new WaitForSeconds (spawnSpeed_);
		}
		Debug.Log ("Spawner done");
	}
}

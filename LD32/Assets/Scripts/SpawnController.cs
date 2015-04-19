using UnityEngine;
using System.Collections;


public class SpawnController : MonoBehaviour {

	public AnimationCurve hazardCountCurve;
	public AnimationCurve spawnSpeedCurve;
	public AnimationCurve moveSpeedCurve;
	public AnimationCurve typeCountCurve;
	public AnimationCurve groupCountCurve;
	public AnimationCurve groupSpawnSpeed;
	public GameObject hazard;
	public Vector3 SpawnPos;
	public  int hazardCount;
	public int currentWaveID = 1;
	public bool isActive = false;
	// private
	private int numPerGroup ;
	private int numHazardTypes; 
	private GameController gameController;
	private int maxTypes = 6;
	private float speed;
	private float spawnSpeed_;
	private int groupCount;
	private float groupSpawnWait;

	// Use this for initialization
	void Start () 
	{
		GameObject gameControllerObj = GameObject.FindWithTag ("GameController");
		if (gameControllerObj != null) 
			gameController = gameControllerObj.GetComponent<GameController> ();
		else 
			Debug.Log ("Spawn controller cannot find GameController!");
	}
	Identifier PickType()
	{
		/*
		 * 	XXX - Temporary WIP
	 	 */
		int typeID = Random.Range(0,numHazardTypes);
		return new Identifier(typeID); 
	}
	void SpawnHazard(GameObject obj)
	{
		Vector3 spawnPos = new Vector3 (Random.Range (-SpawnPos.x, SpawnPos.x), SpawnPos.y, SpawnPos.z);
		GameObject enemyObject = Instantiate (obj, spawnPos, Quaternion.identity) as GameObject;
		enemyObject.GetComponent<EnemyBehavior>().SetTypeSpeedAndController(PickType(), speed,  this, gameController);
		//Debug.Log ("enemyObject = " + enemyObject.transform.position);
		++hazardCount;
		Debug.Log ("Active spawn enemey count=" + hazardCount);
	}
	// Update is called once per frame
	void Update() {
	
	}
	// count to spawn in the wave and the number of enemey types
	public void KickWave(int count, int numTypes, float moveSpeed, float spawnSpeed)
	{
		if (numTypes > maxTypes)
			numTypes = maxTypes;
		hazardCount = 0;
		numPerGroup = count;
		numHazardTypes = numTypes;
		speed = moveSpeed;
		spawnSpeed_ = spawnSpeed;
	
		StartCoroutine (SpawnMain ());
	}
	public void KickWave(int waveID)
	{
		isActive = true;
		currentWaveID = waveID;
		int numHazards = (int)hazardCountCurve.Evaluate(waveID);
		int numTypes = (int)typeCountCurve.Evaluate(waveID);
		float speed = moveSpeedCurve.Evaluate(waveID);
		float spawnSpeed = spawnSpeedCurve.Evaluate(waveID);
		groupCount = (int)groupCountCurve.Evaluate(waveID);
		groupSpawnWait = groupSpawnSpeed.Evaluate(waveID);
		Debug.Log ("Kick Wave #" + waveID + " Enemies= " + numHazards + " typePerGroup=" + numTypes + " speed = " 
		           + speed + " spawnSpeed = " + spawnSpeed + " groupCount=" + groupCount + " GroupDelay=" + groupSpawnWait);

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
		Debug.Log ("***** Spawn started, num groups=" + groupCount + " num per group = " + numPerGroup + " total to spawn this wave = " + groupCount * numPerGroup);
		for (int i=0;i<groupCount && !gameController.IsGameOver();i++)
		{
			// group delay
			if (i!=0)
				yield return new WaitForSeconds(groupSpawnWait);
			// spawn group
			Debug.Log ("Wave " + currentWaveID + " Spawn Group idx " + i + " count=" + groupCount);
			for (int j=0;j<numPerGroup && !gameController.IsGameOver();j++)
			{
				SpawnHazard(hazard);
				yield return new WaitForSeconds (spawnSpeed_);
			}
			Debug.Log ("Wave " + currentWaveID + " Done Spawn Group idx " + i + " count=" + groupCount);
		}
		Debug.Log ("***** Wave " + currentWaveID + " DONE *****");
		isActive = false;
		gameController.SetKickWave();
	}
}

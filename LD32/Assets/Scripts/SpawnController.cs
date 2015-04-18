using UnityEngine;
using System.Collections;


public class SpawnController : MonoBehaviour {

	public float spawnSpeed = 1f;
	public float speed = 3.0f;
	public GameObject hazard;
	public Color[] colors = new Color[8];
	public bool initColors = true;
	public Vector3 SpawnPos;
	public int hazardCount;
	// private
	public int remainToSpawn ;
	public int numHazardTypes; 
	private GameController gameController;
	private int maxTypes = 6;
	// Use this for initialization
	void Start () 
	{
		GameObject gameControllerObj = GameObject.FindWithTag ("GameController");
		if (gameControllerObj != null) 
		{
			gameController = gameControllerObj.GetComponent<GameController> ();
		} 
		else 
		{
			Debug.Log ("Spawn controller cannot find GameController!");
		}
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

	void SpawnHazard(GameObject obj)
	{
		Vector3 spawnPos = new Vector3 (Random.Range (-SpawnPos.x, SpawnPos.x), SpawnPos.y, SpawnPos.z);
		GameObject enemyObject = Instantiate (obj, spawnPos, Quaternion.identity) as GameObject;
		enemyObject.GetComponent<EnemyBehavior>().SetTypeSpeedAndController(PickType(), speed,  this);
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
		spawnSpeed = spawnSpeed;
		StartCoroutine (SpawnMain ());
	}
	IEnumerator SpawnMain()
	{
		Debug.Log ("Spawne started");
		while (!gameController.gameover && remainToSpawn > 0) 
		{
			SpawnHazard(hazard);
			yield return new WaitForSeconds (spawnSpeed);
		}
		Debug.Log ("Spawner done");
	}
}

using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public Color identity;
	private SpawnController spawner_;
	private float speed = 1.0f;
	~EnemyBehavior()
	{
		--spawner_.hazardCount;
	}
	public void SetTypeSpeedAndController(Color color, float speed, SpawnController spawner)
	{
		identity = color;
		GetComponent<Renderer>().material.color = color;
		Debug.Log ("Speed = " + speed);
		GetComponent<Rigidbody> ().velocity = -transform.forward * speed; // Random.Range(speedMin, speedMax);
		spawner_ =  spawner;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

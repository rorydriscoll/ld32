using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public AnimationCurve speedDecay;
	public Color identity;
	private SpawnController spawner_;
	private float initialSpeed;
	~EnemyBehavior()
	{
		--spawner_.hazardCount;
	}
	public void SetTypeSpeedAndController(Color color, float speed, SpawnController spawner)
	{
		identity = color;
		GetComponent<Renderer>().material.color = color;
		//Debug.Log ("Speed = " + speed);
		GetComponent<Rigidbody> ().velocity = -transform.forward * speed; // Random.Range(speedMin, speedMax);
		initialSpeed = speed;
		spawner_ =  spawner;
	}
	void OnCollisionEnter(Collision other)
	{
		//if (other.gameObject.tag == "Player")
		//	spawner_.SetPlayerDead();
	}
	void FixedUpdate()
	{
		float dist = transform.position.z;
		float decay = speedDecay.Evaluate(dist);
		//Debug.Log("Dist " + dist + " decay = " + decay);
		GetComponent<Rigidbody> ().velocity = -transform.forward * initialSpeed * decay; // Random.Range(speedMin, speedMax);
		//if (spawner_.IsPlayerDead())
		//	DestroyObject(gameObject);
	}
}

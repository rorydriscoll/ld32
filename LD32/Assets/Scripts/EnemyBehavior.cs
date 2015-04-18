using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public AnimationCurve speedDecay;
	public MeshFilter meshA, meshB, meshC, meshD;
	public MeshFilter[] meshes = new MeshFilter[4];
	public Identifier identity;
	private SpawnController spawner_;
	private float initialSpeed;
	~EnemyBehavior()
	{
		--spawner_.hazardCount;
	}
	void Start()
	{
		meshes[0] = meshA;
		meshes[1] = meshB;
		meshes[2] = meshC;
		meshes[3] = meshD;
	}
	public void SetTypeSpeedAndController(Identifier ID, float speed, SpawnController spawner)
	{
		identity = ID;
		GetComponent<Renderer>().material.color = ID.Color;
		//GetComponent<MeshFilter>().mesh = meshes[identity.ID ()];
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
		if (transform.position.z < -20.0f)
			DestroyObject(gameObject);

	}

    void TakeDamage(object o)
    {
        Identifier projectileIdentifier = (Identifier)o;

        Debug.Log("I GOT HIT BY " + projectileIdentifier.l + " by " + projectileIdentifier.r);
    }
}

using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public AnimationCurve speedDecay;
	public Mesh[] AttributeMesh = new Mesh[4];
	public Identifier identity;
	private SpawnController spawner_;
	private float initialSpeed;
	~EnemyBehavior()
	{
		--spawner_.hazardCount;
	}
	public void SetTypeSpeedAndController(Identifier ID, float speed, SpawnController spawner)
	{
		identity = ID;
		Debug.Log ("id = " + ID.ID + " meshid = " + identity.GetMeshID() + " colorid = " + identity.GetColorID());
		//GetComponent<Renderer>().material.color = ID.Color;
		GetComponent<MeshFilter>().mesh = AttributeMesh[identity.GetMeshID()];
		//Debug.Log ("Speed = " + speed);
		GetComponent<Rigidbody> ().velocity = transform.forward * speed; // Random.Range(speedMin, speedMax);
		initialSpeed = speed;
		spawner_ =  spawner;
        transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
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
		GetComponent<Rigidbody> ().velocity = transform.forward * initialSpeed * decay; // Random.Range(speedMin, speedMax);
		//if (spawner_.IsPlayerDead())
		//	DestroyObject(gameObject);
		if (transform.position.z < -20.0f)
			DestroyObject(gameObject);

	}

    void TakeDamage(object o)
    {
        Identifier projectileIdentifier = (Identifier)o;
		bool killed = projectileIdentifier.r == identity.r && projectileIdentifier.l == identity.l;
		Debug.Log("I GOT HIT BY " + projectileIdentifier.l + ", " + projectileIdentifier.r + " (" + projectileIdentifier.ID + ") killed=" + killed);
		if (killed)
			DestroyObject(gameObject);
    }
}

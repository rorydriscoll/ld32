using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public AnimationCurve speedDecay;
	public Mesh[] AttributeMesh = new Mesh[4];
	public Identifier identity;
    public Material transparent;
	private SpawnController spawner_;
	private float initialSpeed;
	public float goalPos = 0f;
	public float destroyGOPos = -11f;
	private GameController gameController;
	~EnemyBehavior()
	{
		--spawner_.hazardCount;
	}
	public void SetTypeSpeedAndController(Identifier ID, float speed, SpawnController spawner, GameController gc)
	{
		identity = ID;
		Debug.Log ("id = " + ID.ID + " meshid = " + identity.GetMeshID() + " colorid = " + identity.GetColorID());
        if (identity.GetColorID() == 2)
        {
            GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.2f);
            GetComponent<Renderer>().material = transparent;
        }

		GetComponent<MeshFilter>().mesh = AttributeMesh[identity.GetMeshID()];
		//Debug.Log ("Speed = " + speed);
		GetComponent<Rigidbody> ().velocity = transform.forward * speed; // Random.Range(speedMin, speedMax);
		initialSpeed = speed;
		spawner_ =  spawner;
		gameController = gc;
		transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
	}
	void FixedUpdate()
	{
		float dist = transform.position.z;
		float decay = speedDecay.Evaluate(dist);
		//Debug.Log("Dist " + dist + " decay = " + decay);
		GetComponent<Rigidbody> ().velocity = transform.forward * initialSpeed * decay; // Random.Range(speedMin, speedMax);
		//if (spawner_.IsPlayerDead())
		//	DestroyObject(gameObject);
		if (transform.position.z < destroyGOPos)
			DestroyObject(gameObject);
		if (transform.position.z < 0f)
			gameController.EnemyReachedGoal();
	}

	void TakeDamage(object o)
	{
		Identifier projectileIdentifier = (Identifier)o;
		bool killed = projectileIdentifier.r == identity.r && projectileIdentifier.l == identity.l;
		Debug.Log("I GOT HIT BY " + projectileIdentifier.l + ", " + projectileIdentifier.r + " (" + projectileIdentifier.ID + ") killed=" + killed);
		if (killed)
		{
			DestroyObject(gameObject);
<<<<<<< HEAD
			gameController.EnemyKilled(identity);
		}
    }
=======
	}
>>>>>>> 6fa579509feae3da21fd01e8110cfce663ccd775
}

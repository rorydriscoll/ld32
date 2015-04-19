using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public AnimationCurve speedDecay;
	public Mesh[] GhostMeshes = new Mesh[Identifier.kNumCreatures];
    public Mesh[] ZombieMeshes = new Mesh[Identifier.kNumCreatures];
	public Identifier identity;
    public Material transparent;
	private float initialSpeed;

    private float timeOffset;
    private float lf;
    private float la;

	public float goalPos = 0f;
	public float destroyGOPos = -11f;
	private GameController gameController;
	private bool reachedGoal = false;
	void Start()
    {
        timeOffset = Random.Range(0.0f, 1.0f);
        lf = Random.Range(8.0f, 13.0f);
        la = Random.Range(0.2f, 1.0f);
    }

	public void SetTypeSpeedAndController(Identifier ID, float speed, SpawnController spawner, GameController gc)
	{
        if (!ID.IsValid)
            Debug.Log("INVALID!");

		identity = ID;
		Debug.Log ("id = " + ID.ID + " meshid = " + identity.MeshID + " colorid = " + identity.ColorID);
        if (identity.ColorID == 1)
        {
            GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.2f);
            GetComponent<Renderer>().material = transparent;
            GetComponent<MeshFilter>().mesh = GhostMeshes[identity.MeshID];
        }
        else
            GetComponent<MeshFilter>().mesh = ZombieMeshes[identity.MeshID];
            
		//Debug.Log ("Speed = " + speed);
		//GetComponent<Rigidbody> ().velocity = transform.forward * speed; // Random.Range(speedMin, speedMax);
		initialSpeed = speed;
		gameController = gc;
		transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
	}
	void Update()
	{
		float dist = transform.position.z;
		float decay = speedDecay.Evaluate(dist);
		//Debug.Log("Dist " + dist + " decay = " + decay);

        float z = transform.position.z - (initialSpeed * decay * Time.deltaTime);
        float y = Mathf.Abs(Mathf.Sin((Time.time + timeOffset) * lf) * la * decay);

        transform.position = new Vector3(transform.position.x, y, z);

        transform.rotation = Quaternion.AngleAxis(180, Vector3.up) * Quaternion.AngleAxis(Mathf.Sin((Time.time + timeOffset) * lf) * la * decay * 5, transform.forward);

		if (transform.position.z < destroyGOPos)
			DestroyObject(gameObject);
		if (!reachedGoal && transform.position.z < 0f)
		{
			reachedGoal = true;
			gameController.EnemyReachedGoal();
		}
	}

	void TakeDamage(object o)
	{
		Identifier projectileIdentifier = (Identifier)o;
		bool killed = projectileIdentifier.r == identity.r && projectileIdentifier.l == identity.l;
		Debug.Log("I GOT HIT BY " + projectileIdentifier.l + ", " + projectileIdentifier.r + " (" + projectileIdentifier.ID + ") killed=" + killed);
		if (killed)
		{
			DestroyObject(gameObject);
			gameController.EnemyKilled(identity);
		}
    }
}

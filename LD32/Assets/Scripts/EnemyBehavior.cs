using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public AnimationCurve speedDecay;
	public Mesh[] PlainMeshes = new Mesh[3];
    public Mesh[] ZombieMeshes = new Mesh[3];
	public Identifier identity;
    public Material transparent;
	private SpawnController spawner_;
	private float initialSpeed;

    private float timeOffset;
    private float lf;
    private float la;

	public float goalPos = 0f;
	public float destroyGOPos = -11f;
	private GameController gameController;

	void Start()
    {
        timeOffset = Random.Range(0.0f, 1.0f);
        lf = Random.Range(8.0f, 13.0f);
        la = Random.Range(0.2f, 1.0f);
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

        if (identity.GetColorID() == 1)
            GetComponent<MeshFilter>().mesh = ZombieMeshes[identity.GetMeshID()];
        else
            GetComponent<MeshFilter>().mesh = PlainMeshes[identity.GetMeshID()];
            
		//Debug.Log ("Speed = " + speed);
		//GetComponent<Rigidbody> ().velocity = transform.forward * speed; // Random.Range(speedMin, speedMax);
		initialSpeed = speed;
		spawner_ =  spawner;
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
			gameController.EnemyKilled(identity);
		}
    }
}

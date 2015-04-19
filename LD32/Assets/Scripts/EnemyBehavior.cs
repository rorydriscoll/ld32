using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public AnimationCurve speedDecay;
	public Mesh[] AttributeMesh = new Mesh[3];
	public Identifier identity;
    public Material transparent;
	private SpawnController spawner_;
	private float initialSpeed;

    private float timeOffset;
    private float lf;
    private float la;
    private float af;
    private float aa;

	~EnemyBehavior()
	{
		--spawner_.hazardCount;
	}
    void Start()
    {
        timeOffset = Random.Range(0.0f, 1.0f);
        lf = Random.Range(5.0f, 15.0f);
        la = Random.Range(1.0f, 3.0f);
        af = Random.Range(5.0f, 15.0f);
        aa = Random.Range(0.5f, 3.0f);
    }

	public void SetTypeSpeedAndController(Identifier ID, float speed, SpawnController spawner)
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

        Vector3 velocity = transform.forward * initialSpeed * decay; // Random.Range(speedMin, speedMax);

        velocity += transform.up * Mathf.Sin((Time.time + timeOffset) * lf) * la * decay;

        GetComponent<Rigidbody>().velocity = velocity;

        Quaternion av = Quaternion.AngleAxis(180, Vector3.up) * Quaternion.AngleAxis(Mathf.Sin((Time.time + timeOffset) * af) * aa, transform.forward);

        GetComponent<Rigidbody>().MoveRotation(av);

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

using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{

    public AnimationCurve speedDecay;
    public Mesh[] GhostMeshes = new Mesh[Identifier.kNumCreatures];
    public Mesh[] ZombieMeshes = new Mesh[Identifier.kNumCreatures];
    public Identifier identity;
    public Material transparent;
    private float initialSpeed;
    private float yBase;

    private float timeOffset;
    private float lf;
    private float la;

    public float goalPos = 0f;
    public float destroyGOPos = -11f;
    private GameController gameController;
    private bool reachedGoal = false;
    private bool dead = false;
    private Rigidbody body;
    
    void Start()
    {
        timeOffset = Random.Range(0.0f, 1.0f);
        lf = Random.Range(8.0f, 10.0f);
        la = Random.Range(0.4f, 0.8f);

        body = GetComponent<Rigidbody>();
    }

    public void SetTypeSpeedAndController(Identifier ID, float speed, SpawnController spawner, GameController gc)
    {
        if (!ID.IsValid)
            Debug.Log("INVALID!");

        identity = ID;
        Debug.Log("id = " + ID.ID + " meshid = " + identity.MeshID + " colorid = " + identity.ColorID);
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
        yBase = spawner.transform.position.y;
        gameController = gc;
        transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
    }
    void Update()
    {
        if (dead)
            UpdateDead();
        else
            UpdateUndead();
    }

    void UpdateUndead()
    {
        float dist = transform.position.z;
        float decay = speedDecay.Evaluate(dist);
        //Debug.Log("Dist " + dist + " decay = " + decay);

        float z = transform.position.z - (initialSpeed * decay * Time.deltaTime);
        float y = yBase + Mathf.Abs(Mathf.Sin((Time.time + timeOffset) * lf) * la * decay);

        body.MovePosition(new Vector3(transform.position.x, y, z));
        body.MoveRotation(Quaternion.AngleAxis(180, Vector3.up) * Quaternion.AngleAxis(Mathf.Sin((Time.time + timeOffset) * lf) * la * decay * 5, transform.forward));

        if (transform.position.z < destroyGOPos && !GetComponent<Renderer>().isVisible)
            DestroyObject(gameObject);

        if (!reachedGoal && transform.position.z < 0f)
        {
            reachedGoal = true;
            gameController.EnemyReachedGoal();
        }
    }

    void UpdateDead()
    {
        if (!RendererExtensions.IsVisibleFrom(GetComponent<Renderer>(), GameObject.FindObjectOfType<Camera>()))
            DestroyObject(gameObject);
    }

    void TakeDamage(object o)
    {
        if (dead)
            return;

        DamageEvent damage = (DamageEvent)o;

        Identifier projectileIdentifier = damage.identifier;
        bool killed = projectileIdentifier.r == identity.r && projectileIdentifier.l == identity.l;
        Debug.Log("I GOT HIT BY " + projectileIdentifier.l + ", " + projectileIdentifier.r + " (" + projectileIdentifier.ID + ") killed=" + killed);
        if (killed)
        {
            dead = true;
            gameController.EnemyKilled(identity);

            Rigidbody body = GetComponent<Rigidbody>();

            Vector3 direction = (transform.position - damage.origin).normalized;

            body.isKinematic = false;
            body.AddForce((direction + Vector3.up) * 60, ForceMode.Impulse);
            body.AddTorque(Random.onUnitSphere * 10000, ForceMode.VelocityChange);
        }
    }
}

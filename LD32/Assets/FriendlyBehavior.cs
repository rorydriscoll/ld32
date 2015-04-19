using UnityEngine;
using System.Collections;

public class FriendlyBehavior : MonoBehaviour
{
    public Mesh[] Meshes = new Mesh[Identifier.kNumCreatures];
    public Identifier identity;
    private float initialSpeed;
    private float yBase;

    private float timeOffset;
    private float lf;
    private float la;

    public float destroyGOPos = 50;

    void Start()
    {
        timeOffset = Random.Range(0.0f, 1.0f);
        lf = Random.Range(10.0f, 13.0f);
        la = Random.Range(0.5f, 1.0f);
    }

    public void SetTypeSpeedAndController(Identifier ID, float speed, SpawnController spawner, GameController gc)
    {
        identity = ID;

        GetComponent<MeshFilter>().mesh = Meshes[identity.MeshID];

        initialSpeed = speed;
        yBase = spawner.transform.position.y;
    }

    void Update()
    {
        Vector3 position = transform.position + transform.forward * initialSpeed * Time.deltaTime;

        float y = yBase + Mathf.Abs(Mathf.Sin((Time.time + timeOffset) * lf) * la);

        transform.position = new Vector3(position.x, y, position.z);
        transform.rotation = Quaternion.AngleAxis(Mathf.Sin((Time.time + timeOffset) * lf) * la * 5, transform.forward);

        if (transform.position.z > destroyGOPos && !GetComponent<Renderer>().isVisible)
            DestroyObject(gameObject);
    }
}

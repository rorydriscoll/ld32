using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    struct Spawner
    {
        public Vector3 min;
        public Vector3 max;

        public float delay;

        public bool fired;
    };

	public GameObject bomb;

	private Spawner[] m_spawners = new Spawner[4];
	private Identifier m_identifier;
	private GameController gameController;
    private float m_timer;
    private bool m_active;

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(transform.position, transform.localScale);
	}

	public void Fire(Identifier identifier)
	{
        if (m_active)
			return;

		m_identifier = identifier;
        m_active = true;

        for (int i = 0; i < m_spawners.Length; ++i)
        {
            m_spawners[i].delay = Random.Range(0.0f, 1.5f);
            m_spawners[i].fired = false;
        }
	}

	void Start() 
	{
		GameObject gameControllerObj = GameObject.FindWithTag ("GameController");

		if (gameControllerObj != null) 
			gameController = gameControllerObj.GetComponent<GameController> ();
		else 
			Debug.Log ("weapon controller cannot find GameController!");

        m_spawners[0].min = new Vector3(-transform.localScale.x, -transform.localScale.y, -transform.localScale.z);
        m_spawners[0].max = new Vector3(0, transform.localScale.y, 0);
        m_spawners[1].min = new Vector3(-transform.localScale.x, -transform.localScale.y, 0);
        m_spawners[1].max = new Vector3(0, transform.localScale.y, transform.localScale.z);
        m_spawners[2].min = new Vector3(0, -transform.localScale.y, -transform.localScale.z);
        m_spawners[2].max = new Vector3(transform.localScale.x, transform.localScale.y, 0);
        m_spawners[3].min = new Vector3(0, -transform.localScale.y, 0);
        m_spawners[3].max = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
    
	void Update()
	{
        if (!gameController.CanFireWeapon())
            m_active = false;

        if (!m_active)
			return;

        m_timer += Time.deltaTime;

        bool active = false;

        for (int i = 0; i < m_spawners.Length; ++i)
        {
            if (m_spawners[i].fired)
                continue;

            active = true;

            if (m_timer < m_spawners[i].delay)
                continue;
            
            SpawnBomb(m_spawners[i].min, m_spawners[i].max);

            m_spawners[i].fired = true;
        }

        m_active = active;
	}

    void SpawnBomb(Vector3 min, Vector3 max)
    {
        float dx = Random.Range(min.x, max.x) * 0.5f;
        float dz = Random.Range(min.z, max.z) * 0.5f;

        GameObject go = GameObject.Instantiate<GameObject>(bomb);

        go.GetComponent<Transform>().position = transform.position + new Vector3(dx, 0, dz);
        go.GetComponent<Transform>().rotation = Random.rotation;
        go.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-20.0f, -30.0f), Random.Range(-2.0f, 2.0f));
        go.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 1000;

        go.SendMessage("SetIdentifier", m_identifier);
    }
}

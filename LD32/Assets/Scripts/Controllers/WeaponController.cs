using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
	public GameObject bomb;

	private bool m_firing;
	private Identifier m_identifier;
	private GameController gameController;

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(transform.position, transform.localScale);
	}

	public void Fire(Identifier identifier)
	{
		if (m_firing)
			return;
		m_identifier = identifier;
		m_firing = true;
	}
	void Start() 
	{
		GameObject gameControllerObj = GameObject.FindWithTag ("GameController");
		if (gameControllerObj != null) 
			gameController = gameControllerObj.GetComponent<GameController> ();
		else 
			Debug.Log ("weapon controller cannot find GameController!");
	}    
	void Update()
	{
		if (!m_firing || !gameController.CanFireWeapon())
		{
			m_firing = false;
			return;
		}

        SpawnBomb(-transform.localScale.x, 0, -transform.localScale.z, 0);
        SpawnBomb(-transform.localScale.x, 0, 0, transform.localScale.z);
        SpawnBomb(0, transform.localScale.x, -transform.localScale.z, 0);
        SpawnBomb(0, transform.localScale.x, 0, transform.localScale.z);

		m_firing = false;
	}

    void SpawnBomb(float xmin, float xmax, float zmin, float zmax)
    {
        float dx = Random.Range(xmin, xmax) * 0.5f;
        float dz = Random.Range(zmin, zmax) * 0.5f;

        GameObject go = GameObject.Instantiate<GameObject>(bomb);

        go.GetComponent<Transform>().position = transform.position + new Vector3(dx, 0, dz);
        go.GetComponent<Transform>().rotation = Random.rotation;
        go.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-20.0f, -30.0f), Random.Range(-2.0f, 2.0f));

        go.SendMessage("SetIdentifier", m_identifier);
    }
}

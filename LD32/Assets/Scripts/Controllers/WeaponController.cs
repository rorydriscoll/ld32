using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
	public GameObject bomb;

	private GameController gameController;

	void OnDrawGizmos()
	{
        float dx = transform.localScale.x * 0.5f;

		Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - transform.right * dx, transform.position + transform.right * dx);
	}

    void Start()
    {
        GameObject gameControllerObj = GameObject.FindWithTag("GameController");

        if (gameControllerObj != null)
            gameController = gameControllerObj.GetComponent<GameController>();
        else
            Debug.Log("weapon controller cannot find GameController!");
    }
    
    public void Fire(Identifier identifier)
	{
        float dx = Random.Range(-transform.localScale.x, transform.localScale.x) * 0.5f;

        GameObject go = GameObject.Instantiate<GameObject>(bomb);

        go.GetComponent<Transform>().position = transform.position + new Vector3(dx, 0, 0);
        go.GetComponent<Transform>().rotation = Random.rotation;
        go.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-20.0f, -30.0f), Random.Range(-2.0f, 2.0f));
        go.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 1000;

        go.SendMessage("SetIdentifier", identifier);
	}
}

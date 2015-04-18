using UnityEngine;
using System.Collections;


[System.Serializable]
public class Boundary
{
	public float xmin; 
	public float xmax;
	public float zmin;
	public float zmax;
}

public class ShipPlayer : MonoBehaviour {

	public float speed;
	public Boundary b;
	public float tilt;
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate = 0.5F;
	private float nextFire = 0.0F;
	// Use this for initialization
	void Start () {
		speed = 10.0f;
		tilt = 3;
	}
	void FixedUpdate() 
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;
		Vector3 pos = GetComponent<Rigidbody> ().position;
	
		pos.x = Mathf.Clamp (pos.x, b.xmin, b.xmax);
		pos.z = Mathf.Clamp (pos.z, b.zmin, b.zmax);
		GetComponent<Rigidbody> ().position = pos;
		GetComponent<Rigidbody> ().rotation = Quaternion.Euler (0, 0, movement.x * -tilt * speed); 

	}
	void Update()
	{
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			//GameObject shotInst = 
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation) ;//as GameObject;
			GetComponent<AudioSource>().Play();

		}
	}
}

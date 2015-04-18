using UnityEngine;
using System.Collections;

public class ballon : MonoBehaviour {

	public Vector3 offset;
	public float dir = 1.0f;
	public float dtime = 1.0f;
	private float curr = 0f;
	public GameObject player;
	private bool isplayer = false;
	public float speed = 0.5f;
	// Use this for initialization
	void Start () {
		//offset = new Vector3(0f,0.1f,0f);
	}
	void OnTriggerEnter(Collider other)
	{
		player.SetActive(false);
		isplayer= true;
		Debug.Log("ballon hit");

	}
	
	// Update is called once per frame
	void Update () {
		curr += Time.deltaTime;
		if (curr > dtime)
		{
			dir *= -1.0f;
			curr = 0f;
		}
		transform.position = transform.position + offset * dir * Time.deltaTime;
		if (isplayer)
		{
			float scale = speed * Time.deltaTime;
			float moveHorizontal = Input.GetAxis ("Horizontal") * scale;
			float moveVertical = Input.GetAxis ("Vertical") * scale;
			transform.position = new Vector3(transform.position.x + moveHorizontal,transform.position.y + moveVertical,transform.position.z);
		}
	}
}

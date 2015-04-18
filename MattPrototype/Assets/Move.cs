using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public float speed = 1.0f;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		float scale = speed * Time.deltaTime;
		float moveHorizontal = Input.GetAxis ("Horizontal") * scale;
		float moveVertical = Input.GetAxis ("Vertical") * scale;
		transform.position = new Vector3(transform.position.x + moveHorizontal,0,transform.position.z+moveVertical);
	}
} 	

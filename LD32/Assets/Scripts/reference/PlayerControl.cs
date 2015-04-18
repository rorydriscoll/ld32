using UnityEngine;
using System.Collections;


public class PlayerControl : MonoBehaviour {

	public GameObject hazard;
	public float speed;
	public float spawnWait; 
	private int count;
	public GUIText countText;
	public GUIText completedText;
	void Start()
	{
		count = 0;
		SetCountText ();
	}
	void SetCountText()
	{
		countText.text = "Count: " + count.ToString ();
		completedText.gameObject.SetActive (false);
	}

	void FixedUpdate() 
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().AddForce (movement * speed * Time.deltaTime);
	}
	void OnTriggerEnter (Collider other) 
	{
		if (other.gameObject.tag == "Pickup") 
		{
			other.gameObject.SetActive(false);
			++count;
			SetCountText();
			if (count == 13)
			{
				completedText.gameObject.SetActive(true);
			}
		}
	}

}

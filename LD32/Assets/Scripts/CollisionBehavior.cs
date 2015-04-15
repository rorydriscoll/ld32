using UnityEngine;
using System.Collections;

public class CollisionBehavior : MonoBehaviour {

	AudioSource audioSource;
	void Start()
	{
		audioSource = GetComponent<AudioSource> ();
	}
	void OnCollisionEnter(Collision collision)
	{
		if (tag == "ground" || collision.gameObject.tag == "ground")
			return;
		Debug.Log ("collision " + gameObject.name +" other = " + collision.gameObject.name);
		audioSource.Play ();

	}
}

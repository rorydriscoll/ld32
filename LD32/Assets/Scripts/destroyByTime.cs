using UnityEngine;
using System.Collections;

public class destroyByTime : MonoBehaviour {

	public float lifetime;
	// Use this for initialization
	void Start () {

		Destroy (gameObject, lifetime);
	}

}

using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	public float speedMin;
	public float speedMax;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().velocity = -transform.forward * Random.Range(speedMin, speedMax);
	}

}

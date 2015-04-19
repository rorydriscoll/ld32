using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {

	public float tumble;
	void Start()
	{
		//GetComponent()<RigidBody>.angularVelocity = Random.insideUnitSphere() * tumble;
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble * Random.value;
	}
}

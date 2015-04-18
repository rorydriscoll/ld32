using UnityEngine;
using System.Collections;

public class RandomSize : MonoBehaviour {

	public float minSize;
	public float maxSize;

	void Start () {
		float s = 1.0f;	
		if (Random.value > 0.95f)
			s = Random.Range (3.0f, 5.0f);
		else
			s = Random.Range (0.25f, 2.0f);
		transform.localScale = new Vector3(s,s,s);
	}

}

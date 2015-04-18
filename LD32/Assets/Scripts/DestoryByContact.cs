using UnityEngine;
using System.Collections;

public class DestoryByContact : MonoBehaviour {

	public GameObject explosion;
	public GameObject player_explosion;
	public int hitPoints;
	private GameController gameController;

	void Start()
	{
		GameObject gameControllerObj = GameObject.FindWithTag ("GameController");
		if (gameControllerObj != null) {
			gameController = gameControllerObj.GetComponent<GameController> ();
		} else {
			Debug.Log ("Cannot find GameController");
		}

	}
	void OnTriggerEnter(Collider other)
	{
		//Debug.Log (other.name);
		if (other.tag == "Boundary")
			return;
		if (other.tag == "Enemy" && gameObject.tag == "Enemy")
			return;
		Destroy(other.gameObject);
		Destroy (gameObject);
		Instantiate (explosion, transform.position, transform.rotation);
		if (other.tag == "Player") {
			Instantiate (player_explosion, other.transform.position, other.transform.rotation);
			gameController.SetGameOver();
		} else {
			gameController.AddScore(hitPoints);
		}

	}

}

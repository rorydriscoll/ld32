using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    public GameObject[] fireFx = new GameObject[Identifier.kNumLeft];

    private bool m_firing;
    private Identifier m_identifier;
	private GameController gameController;

    public void Fire(Identifier identifier)
    {
        if (m_firing)
            return;
        m_identifier = identifier;
        m_firing = true;
    }
	void Start() 
	{
		GameObject gameControllerObj = GameObject.FindWithTag ("GameController");
		if (gameControllerObj != null) 
			gameController = gameControllerObj.GetComponent<GameController> ();
		else 
			Debug.Log ("weapon controller cannot find GameController!");
	}    
	void Update()
    {
        if (!m_firing || !gameController.CanFireWeapon())
		{
			m_firing = false;
            return;
		}
        GameObject go = fireFx[m_identifier.l];
        ParticleSystem ps = go.GetComponent<ParticleSystem>();

        ps.startColor = m_identifier.Color;
        ps.Play();

        go.SendMessage("SetIdentifier", m_identifier);

        m_firing = false;
    }
}

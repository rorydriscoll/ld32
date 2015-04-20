using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{
    public float m_heatDissipationRate = 0.1f;  // Heat lost per second
    public float m_heatPerShot = 1.0f;          // Heat added per shot
    public float m_heatThreshold = 10.0f;       // Threshold at which the weapon will be overheated
    public float m_overheatTime = 3.0f;         // Time it takes for the weapon to cool down after overheat

    public WeaponController weaponController;
    public GameObject mainCamera;
    public AudioClip[] chargeSounds;

    private bool m_overheating = false;
    private float m_heat;
    private float m_timer;
	private GameController gameController;

    void Start()
    {
		Debug.Log ("Player Behavior start");
        GameObject gameControllerObj = GameObject.FindWithTag ("GameController");
		if (gameControllerObj != null) 
			gameController = gameControllerObj.GetComponent<GameController> ();
		else 
			Debug.Log ("weapon controller cannot find GameController!");
    }

    void Update()
    {
		if (!gameController.CanFireWeapon())
			return;

        m_heat = Mathf.Max(0.0f, m_heat - Time.deltaTime * m_heatDissipationRate);

        if (m_overheating)
            UpdateOverheating();
        else
            UpdateActive();
    }

    void UpdateActive()
    {
        Identifier id = GetFireId();

        if (!id.IsValid)
            return;

        GetComponent<AudioSource>().PlayOneShot(chargeSounds[id.ID]);

        weaponController.Fire(id);

        m_heat = Mathf.Min(m_heatThreshold, m_heat + m_heatPerShot);

        if (m_heat >= m_heatThreshold)
        {
            m_timer = 0;
            m_overheating = true;
        }
    }

    void UpdateOverheating()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= m_overheatTime)
        {
            m_heat = m_heatThreshold * 0.5f;
            m_overheating = false;
        }
    }

    private Identifier GetFireId()
    {
        if (Input.GetButtonDown("L1"))
            return new Identifier(0);
        if (Input.GetButtonDown("L2"))
            return new Identifier(2);
        if (Input.GetButtonDown("L3"))
            return new Identifier(4);

        if (Input.GetButtonDown("R1"))
            return new Identifier(1);
        if (Input.GetButtonDown("R2"))
            return new Identifier(3);
        if (Input.GetButtonDown("R3"))
            return new Identifier(5);

        return Identifier.Invalid;
    }
}

using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{
    enum State
    {
        Idle,
        Charging,
        Reloading,
        Overheating
    }

    public float m_chargeTime = 1;              // Time it takes to charge a shot

    public float m_heatDissipationRate = 0.1f;  // Heat lost per second
    public float m_heatPerShot = 1.0f;          // Heat added per shot
    public float m_heatThreshold = 10.0f;       // Threshold at which the weapon will be overheated

    public float m_reloadTime = 0.5f;           // Time it takes to reload the weapon
    public float m_overheatTime = 3.0f;         // Time it takes for the weapon to cool down after overheat

    public WeaponController weaponController;
    public GameObject mainCamera;

    private State m_mode = State.Idle;
    private float m_heat;
    private float m_timer;
    private Identifier m_identifier = Identifier.Invalid;
    private Quaternion m_cameraRotation;
	private GameController gameController;
    void Start()
    {
		Debug.Log ("Player Behavior start");
		m_cameraRotation = mainCamera.transform.rotation;
		GameObject gameControllerObj = GameObject.FindWithTag ("GameController");
		if (gameControllerObj != null) 
			gameController = gameControllerObj.GetComponent<GameController> ();
		else 
			Debug.Log ("weapon controller cannot find GameController!");
    }

    void Update()
    {
		if (!gameController.CanFireWeapon())
		{
			Debug.Log ("Weapons disabled");
			m_timer = 0f;
			return;
		}

        m_timer += Time.deltaTime;

        UpdateHeat();
        UpdateIdentifier();
		Debug.Log ("mode = " + m_mode);
        switch (m_mode)
        {
            case State.Idle:
                UpdateIdle();
                break;

            case State.Charging:
                UpdateCharging();
                break;

            case State.Reloading:
                UpdateReloading();
                break;

            case State.Overheating:
                UpdateOverheating();
                break;
        }
    }

    void EnterState(State mode)
    {
        m_timer = 0;
        m_mode = mode;
    }

    void UpdateIdle()
    {
        if (m_identifier.IsValid)
            EnterState(State.Charging);
		else
			Debug.Log("id not valid id=" +m_identifier.ID);
    }

    void UpdateCharging()
    {
        if (!m_identifier.IsValid)
        {
			Debug.Log ("identifier not valid? ");
			EnterState(State.Idle);
            return;
        }
		Debug.Log ("Charging weapon");
        float frequency = 100;
        float amplitude = m_timer * 5;

        mainCamera.transform.rotation *= Quaternion.AngleAxis(Mathf.Sin(m_timer * frequency) * amplitude, new Vector3(0, 0, 1));

        if (m_timer >= m_chargeTime)
        {
            weaponController.Fire(m_identifier);

            m_heat = Mathf.Min(m_heatThreshold, m_heat + m_heatPerShot);

            if (m_heat >= m_heatThreshold)
                EnterState(State.Overheating);
            else
                EnterState(State.Reloading);

            mainCamera.transform.rotation = m_cameraRotation;
        }
    }

    void UpdateReloading()
    {
        if (m_timer >= m_reloadTime)
            EnterState(State.Idle);
    }

    void UpdateOverheating()
    {
        if (m_timer >= m_overheatTime)
        {
            m_heat = m_heatThreshold * 0.5f;
            EnterState(State.Reloading);
        }
    }

    private void UpdateHeat()
    {
        m_heat = Mathf.Max(0.0f, m_heat - Time.deltaTime * m_heatDissipationRate);
    }

    private void UpdateIdentifier()
    {
        if (!Input.GetButton("L1") && !Input.GetButton("L2") && !Input.GetButton("L3"))
            m_identifier.l = -1;

        if (!Input.GetButton("R1") && !Input.GetButton("R2") && !Input.GetButton("R3"))
            m_identifier.r = -1;

        if (Input.GetButtonDown("L1"))
            m_identifier.l = 0;
        if (Input.GetButtonDown("L2"))
            m_identifier.l = 1;
        if (Input.GetButtonDown("L3"))
            m_identifier.l = 2;

        if (Input.GetButtonDown("R1"))
            m_identifier.r = 0;
        if (Input.GetButtonDown("R2"))
            m_identifier.r = 1;
        if (Input.GetButtonDown("R3"))
            m_identifier.r = 2;
		Debug.Log ("Update ident = " + m_identifier.ID);
    }
}

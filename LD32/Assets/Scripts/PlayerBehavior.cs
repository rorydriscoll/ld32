using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{
    public float chargeDuration = 1;
    public WeaponController weaponController;
    public GameObject mainCamera;

    private bool charging;
    private float charge;
    private Identifier identifier = Identifier.Invalid;

    void Update()
    {
        UpdateIdentifier();

        if (!charging && identifier.IsValid)
            charging = true;

        if (charging && !identifier.IsValid)
            charging = false;

        if (charging)
        {
            charge += Time.deltaTime / chargeDuration;

            float frequency = 100;
            float amplitude = charge * 5;

            mainCamera.transform.rotation *= Quaternion.AngleAxis(Mathf.Sin(charge * frequency) * amplitude, new Vector3(0, 0, 1));

            if (charge >= 1)
            {
                FireShot();
                charging = false;
                charge = 0;
                identifier.l = identifier.r = 0;
            }
        }
        else
            charge = 0;
    }

    void FireShot()
    {
        weaponController.Fire(identifier);

        mainCamera.transform.rotation = Quaternion.Euler(35.56804f, 346.827f, 355.7559f);
    }

    private void UpdateIdentifier()
    {
        if (!Input.GetButton("L1") && !Input.GetButton("L2") && !Input.GetButton("L3"))
            identifier.l = -1;

        if (!Input.GetButton("R1") && !Input.GetButton("R2") && !Input.GetButton("R3"))
            identifier.r = -1;

        if (Input.GetButtonDown("L1"))
            identifier.l = 0;
        if (Input.GetButtonDown("L2"))
            identifier.l = 1;
        if (Input.GetButtonDown("L3"))
            identifier.l = 2;

        if (Input.GetButtonDown("R1"))
            identifier.r = 0;
        if (Input.GetButtonDown("R2"))
            identifier.r = 1;
        if (Input.GetButtonDown("R3"))
            identifier.r = 2;
    }
}

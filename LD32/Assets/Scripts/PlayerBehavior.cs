using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{
    public float chargeDuration = 1;
    public ParticleSystem fireFx;
    public GameObject mainCamera;

    private bool charging;
    private float charge;
    private Identifier identifier;

    void Update()
    {
        UpdateIdentifier();

        if (!charging && (identifier.l != 0 || identifier.r != 0))
            charging = true;

        if (charging && identifier.l == 0 && identifier.r == 0)
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
        // Map identifiers to colors
        //fireFx.startColor = color;
        fireFx.transform.position = new Vector3(0, 2, 0);
        fireFx.Play();

        mainCamera.transform.rotation = Quaternion.Euler(35.56804f, 346.827f, 355.7559f);
    }

    private void UpdateIdentifier()
    {
        if (identifier.l == 1 && Input.GetButtonUp("L1"))
            identifier.l = 0;
        if (identifier.l == 2 && Input.GetButtonUp("L2"))
            identifier.l = 0;
        if (identifier.l == 3 && Input.GetButtonUp("L3"))
            identifier.l = 0;

        if (identifier.r == 1 && Input.GetButtonUp("R1"))
            identifier.r = 0;
        if (identifier.r == 2 && Input.GetButtonUp("R2"))
            identifier.r = 0;
        if (identifier.r == 3 && Input.GetButtonUp("R3"))
            identifier.r = 0;

        if (Input.GetButtonDown("L1"))
            identifier.l = 1;
        if (Input.GetButtonDown("L2"))
            identifier.l = 1;
        if (Input.GetButtonDown("L3"))
            identifier.l = 1;

        if (Input.GetButtonDown("R1"))
            identifier.r = 1;
        if (Input.GetButtonDown("R2"))
            identifier.r = 2;
        if (Input.GetButtonDown("R3"))
            identifier.r = 3;
    }
}

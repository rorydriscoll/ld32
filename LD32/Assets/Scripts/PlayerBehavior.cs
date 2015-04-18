using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{
    public float chargeDuration = 1;

    private bool charging;
    private float charge;
    private Color color;

    void Update()
    {
        UpdateColor();

        if (Input.GetButtonDown("Fire"))
            charging = true;

        if (Input.GetButtonUp("Fire"))
            charging = false;

        if (charging)
        {
            charge += Time.deltaTime / chargeDuration;

            if (charge >= 1)
            {
                FireShot();
                charging = false;
                charge = 0;
                color = Color.black;
            }
        }
        else
            charge = 0;

        float frequency = 50;
        float amplitude = charge * 20;

        transform.rotation = Quaternion.AngleAxis(Mathf.Sin(charge * frequency) * amplitude, new Vector3(0, 0, 1));
    }

    void FireShot()
    {
    }

    private void UpdateColor()
    {
        if (Input.GetButtonDown("Red"))
            color.r = 1.0f;

        if (Input.GetButtonUp("Red"))
            color.r = 0.0f;

        if (Input.GetButtonDown("Green"))
            color.g = 1.0f;

        if (Input.GetButtonUp("Green"))
            color.g = 0.0f;

        if (Input.GetButtonDown("Blue"))
            color.b = 1.0f;

        if (Input.GetButtonUp("Blue"))
            color.b = 0.0f;

        GetComponent<Renderer>().material.color = color;
    }
}

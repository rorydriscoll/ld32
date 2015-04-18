using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public ParticleSystem fireFx;

    private bool m_firing;
    private Identifier m_identifier;

    public void Fire(Identifier identifier)
    {
        if (m_firing)
            return;

        m_identifier = identifier;
        m_firing = true;
    }

    void Update()
    {
        if (!m_firing)
            return;

        fireFx.startColor = m_identifier.GetColor();
        fireFx.transform.position = new Vector3(0, 2, 0);
        fireFx.Play();

        m_firing = false;
    }
}

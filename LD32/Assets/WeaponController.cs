using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    public List<ParticleSystem> fireFx = new List<ParticleSystem>(4);

    private bool m_firing;
    private Identifier m_identifier;

    public Mesh test;

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

        fireFx[m_identifier.l].startColor = m_identifier.GetColor();
        fireFx[m_identifier.l].Play();

        m_firing = false;
    }
}

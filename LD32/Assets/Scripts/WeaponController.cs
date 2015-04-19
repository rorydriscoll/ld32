using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    public GameObject[] fireFx = new GameObject[Identifier.kNumCreatures];

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

        GameObject go = fireFx[m_identifier.l];
        ParticleSystem ps = go.GetComponent<ParticleSystem>();

        ps.Play();

        go.SendMessage("SetIdentifier", m_identifier);

        m_firing = false;
    }
}

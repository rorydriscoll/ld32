using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ParticleBeam : MonoBehaviour
{
    public float frequency = 5;
    public float amplitude = 1;
    public float speed = 10;

    ParticleSystem m_particleSystem;
    ParticleSystem.Particle[] m_particles;

    void Start()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        m_particles = new ParticleSystem.Particle[m_particleSystem.maxParticles];
    }

    void LateUpdate()
    {
        if (m_particleSystem == null)
            return;

        int count = m_particleSystem.GetParticles(m_particles);

        for (int i = 0; i < count; ++i)
        {
            float l = (m_particles[i].startLifetime - m_particles[i].lifetime) / m_particles[i].startLifetime;
            float f = l * frequency;
            float a = l * amplitude;

            m_particles[i].position = new Vector3(Mathf.Sin(f) * a, Mathf.Cos(f) * a * 0.5f, l * speed);
            m_particles[i].size = Mathf.Max(0.1f, l * l);
        }

        m_particleSystem.SetParticles(m_particles, count);
    }
}

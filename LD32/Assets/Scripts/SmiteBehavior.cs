using UnityEngine;
using System.Collections;

public class SmiteBehavior : MonoBehaviour 
{
    Identifier m_identifier;

    void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleCollisionEvent[] events = new ParticleCollisionEvent[100];

        int count = ps.GetCollisionEvents(other, events);

        for (int i = 0; i < count; ++i)
        {
            ApplySplashDamage(events[i].intersection);
        }
    }

    void ApplySplashDamage(Vector3 position)
    {
        float radius = 3.0f;

        Collider[] hitColliders = Physics.OverlapSphere(position, radius);

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            if (hitColliders[i].tag == "Enemy")
                hitColliders[i].SendMessage("TakeDamage", m_identifier);
        }
    }

    void SetIdentifier(object o)
    {
        m_identifier = (Identifier)o;
    }
}

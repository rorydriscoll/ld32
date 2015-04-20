using UnityEngine;
using System.Collections;

public struct DamageEvent
{
    public Identifier identifier;
    public Vector3 origin;
};

public class SmiteBehavior : MonoBehaviour 
{
    public float damageRadius = 10.0f;
    public GameObject explosionEffect;

    Identifier m_identifier;

    void OnDestroy()
    {
        ApplySplashDamage(transform.position);
    }

    //void OnParticleCollision(GameObject other)
    //{
    //    ParticleSystem ps = GetComponent<ParticleSystem>();
    //    ParticleCollisionEvent[] events = new ParticleCollisionEvent[100];

    //    int count = ps.GetCollisionEvents(other, events);

    //    for (int i = 0; i < count; ++i)
    //    {
    //        if (explosionEffect != null)
    //            GameObject.Instantiate(explosionEffect, events[i].intersection, Quaternion.identity);

    //        ApplySplashDamage(events[i].intersection);
    //    }
    //}

    void ApplySplashDamage(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, damageRadius);

        DamageEvent damage = new DamageEvent();

        damage.identifier = m_identifier;
        damage.origin = position;

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            if (hitColliders[i].tag == "Enemy")
                hitColliders[i].SendMessage("TakeDamage", damage);
        }
    }

    void SetIdentifier(object o)
    {
        m_identifier = (Identifier)o;
    }
}

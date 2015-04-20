using UnityEngine;
using System.Collections;

public class BombBehavior : MonoBehaviour
{
    public float radius = 10.0f;

    private float m_ttl;
    private Identifier m_identifier;

    void Start()
    {
        m_ttl = Random.Range(1.0f, 2.0f);
    }

    void SetIdentifier(object o)
    {
        m_identifier = (Identifier)o;
    }

    void Update()
    {
        m_ttl -= Time.deltaTime;

        if (m_ttl <= 0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

            DamageEvent damage = new DamageEvent();

            damage.identifier = m_identifier;
            damage.origin = transform.position;

            for (int i = 0; i < hitColliders.Length; ++i)
            {
                if (hitColliders[i].tag == "Enemy")
                    hitColliders[i].SendMessage("TakeDamage", damage);
            }

            DestroyObject(gameObject);
        }
    }


}

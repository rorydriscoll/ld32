﻿using UnityEngine;
using System.Collections;

public class BombBehavior : MonoBehaviour
{
    public float radius = 10.0f;
    public Mesh[] variations;
    public GameObject fragment;
    public AudioClip deploySound;
    public AudioClip hitSound;
    public AudioClip[] explodeSounds;

    private float m_ttl;
    private Identifier m_identifier;

    void Start()
    {
        m_ttl = Random.Range(1.0f, 2.0f);

        if (deploySound)
        {
            GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().PlayOneShot(deploySound);
        }
    }

    void SetIdentifier(object o)
    {
        m_identifier = (Identifier)o;
        GetComponent<MeshFilter>().mesh = variations[m_identifier.ID];
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

            float offset = 0.5f;

            SpawnFragment(transform.position + new Vector3(-offset, 0, -offset));
            SpawnFragment(transform.position + new Vector3(-offset, 0, offset));
            SpawnFragment(transform.position + new Vector3(offset, 0, -offset));
            SpawnFragment(transform.position + new Vector3(offset, 0, offset));
            SpawnFragment(transform.position + new Vector3(-offset, 1, -offset));
            SpawnFragment(transform.position + new Vector3(-offset, 1, offset));
            SpawnFragment(transform.position + new Vector3(offset, 1, -offset));
            GameObject fragment = SpawnFragment(transform.position + new Vector3(offset, 1, offset));

            if (explodeSounds.Length > 0)
            {
                fragment.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 0.9f);
                fragment.GetComponent<AudioSource>().PlayOneShot(explodeSounds[Random.Range(0, explodeSounds.Length - 1)]);
            }

            DestroyObject(gameObject);
        }
    }

    GameObject SpawnFragment(Vector3 position)
    {
        GameObject go = GameObject.Instantiate<GameObject>(fragment);

        go.GetComponent<Transform>().position = position;
        go.GetComponent<Transform>().rotation = Random.rotation;

        Vector3 direction = (position - transform.position + Vector3.up).normalized;

        go.GetComponent<Rigidbody>().velocity = direction * 70;

        return go;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hitSound != null)
        {
            GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
            GetComponent<AudioSource>().PlayOneShot(hitSound);
        }
    }
}

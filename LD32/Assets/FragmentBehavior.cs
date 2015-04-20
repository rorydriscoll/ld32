using UnityEngine;
using System.Collections;

public class FragmentBehavior : MonoBehaviour
{
    public Mesh[] variations;

    private float m_timer;

    void Start()
    {
        if (variations.Length > 0)
            GetComponent<MeshFilter>().mesh = variations[Random.Range(0, variations.Length)];
    }

    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer > 2.0f)
            DestroyObject(gameObject);
    }
}

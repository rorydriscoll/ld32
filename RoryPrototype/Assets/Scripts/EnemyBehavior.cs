using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public enum Behavior
    {
        None,
        BeachBall
    }

    private Rigidbody m_rigidbody;
    private Behavior m_behavior = Behavior.None;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        switch (m_behavior)
        {
            case Behavior.BeachBall:
                m_rigidbody.AddForce(m_rigidbody.mass * -Physics.gravity * 0.1f);
                break;

            case Behavior.None:
                break;
        }
    }

    void OnMergeWithBeachBall()
    {
        m_rigidbody.useGravity = false;
        m_behavior = Behavior.BeachBall;
    }
}

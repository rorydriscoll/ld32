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
    private GameObject m_player;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_player = GameObject.Find("Player");
    }

    void DoNormalBehavior()
    {
        Ray ray = new Ray(transform.position, (m_player.transform.position - transform.position).normalized);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.rigidbody)
            {
                GameObject hitobject = hit.rigidbody.gameObject;

                if (hitobject.tag == "Player")
                {
                    Renderer rend = GetComponent<Renderer>();
                    rend.material.color = new Color(1, 0, 1);

                    GetComponent<NavMeshAgent>().SetDestination(m_player.transform.position);
                }
            }
        }
    }

    void DoBeachBallBehavior()
    {
        m_rigidbody.useGravity = false;
        m_rigidbody.MovePosition(transform.position + new Vector3(0, 1, 0) * 1 * Time.deltaTime);
    }

    void Update()
    {
        switch (m_behavior)
        {
            case Behavior.BeachBall:
                DoBeachBallBehavior();
                break;

            case Behavior.None:
                DoNormalBehavior();
                break;
        }
    }

    void OnMergeWithBeachBall()
    {
        GetComponent<NavMeshAgent>().Stop();
        GetComponent<NavMeshAgent>().enabled = false;
        m_behavior = Behavior.BeachBall;
    }
}

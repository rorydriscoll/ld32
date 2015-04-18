using UnityEngine;
using System.Collections;

public class PlayerMotion : MonoBehaviour
{
    public Camera m_camera;

    private Rigidbody m_rigidbody;

    private bool m_haveball;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Move

        float speed = 5.0f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = Quaternion.Euler(0, 45, 0) * new Vector3(x, 0, z).normalized;

        Vector3 velocity = direction * speed;
        Vector3 position = transform.position + velocity * Time.deltaTime;

        m_rigidbody.MovePosition(position);

        // Rotate

        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 lookpoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            m_rigidbody.MoveRotation(Quaternion.LookRotation(lookpoint - transform.position));

            if (Input.GetMouseButtonDown(0))
            {
                // Shoot a ray from the player to the hit point to make sure they can see it

                Ray gunray = new Ray(transform.position, (hit.point - transform.position).normalized);
                RaycastHit gunhit;

                if (Physics.Raycast(gunray, out gunhit))
                {
                    GameObject hitobject = gunhit.rigidbody.gameObject;

                    if (hitobject.tag == "BeachBall")
                    {
                        m_haveball = true;
                        Destroy(hitobject);
                    }

                    if (hitobject.tag == "Enemy")
                    {
                        if (m_haveball)
                        {
                            hitobject.SendMessage("OnMergeWithBeachBall");
                            m_haveball = false;
                        }
                    }
                }
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class TrackBehavior : MonoBehaviour
{
    public GameObject trackObject;

    void Update()
    {
        Vector3 look = (trackObject.transform.position - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(look);
    }
}

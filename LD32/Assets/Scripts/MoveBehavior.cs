using UnityEngine;
using System.Collections;

public class MoveBehavior : MonoBehaviour
{
    private float t;

    void Update()
    {
        t += Time.deltaTime;

        transform.position = new Vector3(Mathf.Sin(t), 0.5f, 0);
    }
}

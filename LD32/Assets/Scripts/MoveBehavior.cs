using UnityEngine;
using System.Collections;

public class MoveBehavior : MonoBehaviour
{
    private float t;
    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        t += Time.deltaTime;

        rigidBody.MovePosition(new Vector3(Mathf.Sin(t) * 2, 0.5f, Mathf.Cos(t) * 2));
    }
}

using UnityEngine;
using System.Collections;

public class SpikesBehavior : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            DestroyObject(col.gameObject);
        }
    }
}

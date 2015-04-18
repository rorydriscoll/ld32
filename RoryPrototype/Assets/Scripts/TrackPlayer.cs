using UnityEngine;
using System.Collections;

public class TrackPlayer : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        transform.position = player.transform.position + new Vector3(-10, 10, -10);
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
    }
}

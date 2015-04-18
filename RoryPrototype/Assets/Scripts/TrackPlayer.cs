using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TrackPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(-10, 10, -10);

    void Update()
    {
        if (player == null)
            return;

        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
    }
}

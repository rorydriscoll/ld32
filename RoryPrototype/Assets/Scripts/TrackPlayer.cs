using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TrackPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(-10, 10, -10);
    public float lookahead = 6.0f;

    void Update()
    {
        if (player == null)
            return;

        Vector3 aimpos = player.transform.position + new Vector3(lookahead, 0, lookahead);

        transform.position = aimpos + offset;
        transform.rotation = Quaternion.LookRotation(aimpos - transform.position);
    }
}

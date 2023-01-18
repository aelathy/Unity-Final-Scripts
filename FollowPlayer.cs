using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 2.7f, -3.27f);
    private AudioSource cameraAudio;
    private float belowBound = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameraAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        if (transform.position.y < belowBound)
        {
            cameraAudio.Stop();
        }
    }
}

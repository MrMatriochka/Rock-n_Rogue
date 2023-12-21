using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;
    public float camSpeed;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, target, camSpeed*Time.deltaTime);

    }
}

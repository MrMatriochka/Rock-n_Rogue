using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;
    public float camSpeed;
    public float dampingDistance;
    Camera mainCamera;

    public GameObject debug;
    void Start()
    {
        offset = transform.position - player.transform.position;
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 target = player.transform.position + offset;
        //transform.position = Vector3.Lerp(transform.position, target, camSpeed*Time.deltaTime);

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            float newDampingDistance = dampingDistance;
            if((pointToLook - player.transform.position).magnitude < dampingDistance)
            {
                //newDampingDistance = (pointToLook - player.transform.position).magnitude;
                newDampingDistance = 0;
            }
            Vector3 target = (pointToLook- player.transform.position).normalized * newDampingDistance+ offset + player.transform.position;

            transform.position = Vector3.Lerp(transform.position, target, camSpeed*Time.deltaTime);
            
        }

    }
}

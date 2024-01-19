using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;
    public float camSpeed;
    public float dampingDivider;
    public float dampingMaxRange;
    public float deadZone;
    Camera mainCamera;
    public GameObject camTarget;
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
            float newDampingDistance = (dampingMaxRange - deadZone) / dampingDivider;

            if ((pointToLook - player.transform.position).magnitude < dampingMaxRange)
            {
                newDampingDistance = ((pointToLook - player.transform.position).magnitude-deadZone) / dampingDivider;
            }
            if ((pointToLook - player.transform.position).magnitude < deadZone)
            {
                newDampingDistance = 0;
            }
            
            Vector3 target = (pointToLook- player.transform.position).normalized * newDampingDistance+ offset + player.transform.position;

            camTarget.transform.position = target-offset;
            //transform.position = Vector3.Lerp(transform.position, target, camSpeed*Time.deltaTime);
            
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.0f;
    public int hp;
    public GameObject bulletSpawnPoint;
    public GameObject bulletPrefab;
    public GameObject particlesPrefab;
    Camera mainCamera;
    Animator anim;
    CharacterController controller;
    public float dashForce;
    public float dashDuration;
    void Start()
    {
        mainCamera = Camera.main;
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //move
        float moveHorizontal = -Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveVertical, 0.0f, moveHorizontal);

        controller.Move(movement * speed * Time.deltaTime);

        //anim
        float dotProductZ = Vector3.Dot(movement.normalized, transform.forward);
        float dotProductX = Vector3.Dot(movement.normalized, transform.right);
        print(new Vector2(dotProductZ, dotProductX));
        anim.SetFloat("MoveZ", dotProductZ);
        anim.SetFloat("MoveX", dotProductX);

        //dash
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }
        

        //rotate
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    public GameObject dashParticles;
    private IEnumerator Dash()
    {
        float moveHorizontal = -Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveVertical, 0.0f, moveHorizontal).normalized;
        if(movement.magnitude==0)
        {
            movement = transform.forward;
        }

        Vector3 currentPos = transform.position;
        Vector3 Gotoposition = currentPos+movement*dashForce;
        float elapsedTime = 0;

        Instantiate(dashParticles, transform.position, Quaternion.identity);
        while (elapsedTime < dashDuration)
        {
            transform.position = Vector3.Lerp(currentPos, Gotoposition, (elapsedTime / dashDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Instantiate(dashParticles, transform.position, Quaternion.identity);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HurtPlayer"))
        {
            Destroy(other);
            hp --;
        }
    }
}

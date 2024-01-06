using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public float speed = 0.0f;
    public float rateOfFire;
    float shootTimer;
    bool canShoot;
    public float rollCd;
    bool canRoll;
    float rollTimer;
    public int hp;
    public GameObject bulletSpawnPoint;
    public GameObject bulletPrefab;
    public GameObject particlesPrefab;
    Camera mainCamera;
    Animator anim;
    CharacterController controller;
    public int ammoMax;
    int ammo;
    public TMP_Text hpUI;
    public TMP_Text ammoUI;
    public GameObject reloadCanvas;
    public GameObject cursor;

    [HideInInspector]public bool canMove = true;
    [HideInInspector]public bool canRotate = true;
    void Start()
    {
        mainCamera = Camera.main;
        anim = transform.GetChild(0).GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        ammo = ammoMax;
        hpUI.text = "x " + hp;
        ammoUI.text = ammo+"/" + ammoMax;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        canMove = true;
        canRotate = true;
    }

    void Update()
    {
        cursor.transform.position = Input.mousePosition;

        //move
        if (canMove)
        {
            float moveHorizontal = -Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3(moveVertical, 0.0f, moveHorizontal).normalized;

            controller.Move(movement * speed * Time.deltaTime);

            //anim
            if (movement != Vector3.zero)
            {
                anim.SetBool("IsMoving", true);
            }
            else { anim.SetBool("IsMoving", false); }

            float dotProductZ = Vector3.Dot(movement.normalized, transform.forward);
            float dotProductX = Vector3.Dot(movement.normalized, transform.right);
            anim.SetFloat("MoveZ", dotProductZ);
            anim.SetFloat("MoveX", dotProductX);
        }
        

       

        //dash
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Roll();
        }


        //rotate
        if (canRotate)
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }
        

        //shoot
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot(true);
        }

        //shoot cd
        if (!canShoot)
        {
            shootTimer -= Time.deltaTime;
            if(shootTimer<=0)
            {
                canShoot = true;
            }
        }

        //roll cd
        if (!canRoll)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0)
            {
                canRoll = true;
            }
        }

        //reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    
    private void Roll()
    {
        if(canRoll)
        {
            float moveHorizontal = -Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            Vector3 pointToLook = new Vector3(moveVertical, 0.0f, moveHorizontal).normalized;
            if (pointToLook.magnitude == 0)
            {
                pointToLook = transform.forward;
            }
            pointToLook += transform.position;
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));

            canMove = false;
            canRotate = false;
            anim.SetTrigger("Roll");

            canRoll = false;
            rollTimer = rollCd;
        }
    }

    public void Shoot(bool hit)
    {
        if (canShoot && ammo>0)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            ammo--;
            if (!hit)
            {
                Instantiate(particlesPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            }
            canShoot = false;
            shootTimer = rateOfFire;
            ammoUI.text = ammo + "/" + ammoMax;
        }
        
    }

    void Reload()
    {
        reloadCanvas.SetActive(true);
        Time.timeScale = 0.2f;
    }

    public void Reload(int bullet)
    {
        if (ammo < ammoMax)
        {
            ammo += bullet;
            ammoUI.text = ammo + "/" + ammoMax;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HurtPlayer"))
        {
            Destroy(other);
            hp --;
            hpUI.text = "x " + hp;
        }
    }
}

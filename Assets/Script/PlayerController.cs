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
    public float dashCd;
    bool canDash;
    float dashTimer;
    public int hp;
    public GameObject bulletSpawnPoint;
    public GameObject bulletPrefab;
    public GameObject particlesPrefab;
    public GameObject dashParticles;
    Camera mainCamera;
    Animator anim;
    CharacterController controller;
    public float dashDistance;
    public float dashDuration;
    public int ammoMax;
    int ammo;
    public TMP_Text hpUI;
    public TMP_Text ammoUI;
    public GameObject reloadCanvas;
    public GameObject cursor;
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
    }

    void Update()
    {
        cursor.transform.position = Input.mousePosition;

        //move
        float moveHorizontal = -Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveVertical, 0.0f, moveHorizontal).normalized;

        controller.Move(movement * speed * Time.deltaTime);

        //anim
        if(movement != Vector3.zero)
        {
            anim.SetBool("IsMoving",true);
        }
        else { anim.SetBool("IsMoving", false); }

        float dotProductZ = Vector3.Dot(movement.normalized, transform.forward);
        float dotProductX = Vector3.Dot(movement.normalized, transform.right);
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

        //dash cd
        if (!canDash)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                canDash = true;
            }
        }

        //reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    
    private IEnumerator Dash()
    {
        if(canDash)
        {
            float moveHorizontal = -Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3(moveVertical, 0.0f, moveHorizontal).normalized;
            if (movement.magnitude == 0)
            {
                movement = transform.forward;
            }

            Vector3 currentPos = transform.position;
            Vector3 Gotoposition = currentPos + movement * dashDistance;
            float elapsedTime = 0;

            //Instantiate(dashParticles, transform.position, Quaternion.identity);
            while (elapsedTime < dashDuration)
            {
                transform.position = Vector3.Lerp(currentPos, Gotoposition, (elapsedTime / dashDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            //Instantiate(dashParticles, transform.position, Quaternion.identity);
            canDash = false;
            dashTimer = dashCd;
            yield return null;
        }
        yield return null;
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

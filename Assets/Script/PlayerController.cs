using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
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
    public GameObject bulletHitPrefab;
    public GameObject bulletMissedPrefab;
    public GameObject particlesPrefab;
    Camera mainCamera;
    Animator anim;
    CharacterController controller;
    public int ammoMax;
    int ammo;
    public TMP_Text hpUI;
    public TMP_Text ammoUI;
    public GameObject reloadCanvas;
    public Slider reloadBar;
    public float reloadTime;
    bool reloading;
    public GameObject cursor;

    [HideInInspector]public bool canMove = true;
    [HideInInspector]public bool canRotate = true;

    public CinemachineVirtualCamera cineCam;
    public float shakeIntensity;
    public float shakeTime;

    [Header("SFX")]
    public AudioClip shootClip;
    public AudioClip noAmmoClip;
    public AudioClip reloadClip;
    AudioSource audio;

    void Start()
    {
        mainCamera = Camera.main;
        anim = transform.GetChild(0).GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        audio = GetComponent<AudioSource>();

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
        cursor.transform.position = Input.mousePosition+ Vector3.up*100;

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
        if (Input.GetKeyDown(KeyCode.Space) && canRoll)
        {
            //Roll();
            StartCoroutine(Dash());
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
        if (Input.GetButtonDown("Fire1") && !reloading && canShoot)
        {
            //Shoot(true);
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
            StartCoroutine(Reload(ammoMax));
        }
    }

    
    private void Roll()
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

    public float dashTime;
    public float dashPower;
    public GameObject dashParticles;
    private IEnumerator Dash()
    {
        Instantiate(dashParticles, transform.position+Vector3.up, Quaternion.identity);
        transform.GetChild(0).gameObject.SetActive(false);
        float elapsedTime = 0;
        float waitTime = dashTime;

        float moveHorizontal = -Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 dashDir = new Vector3(moveVertical, 0.0f, moveHorizontal).normalized;
        if (dashDir.magnitude == 0)
        {
            dashDir = transform.forward;
        }

        Vector3 currentPos = transform.position;
        Vector3 Gotoposition = currentPos + dashDir*dashPower;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPos, Gotoposition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //Instantiate(dashParticles, transform.position + Vector3.up, Quaternion.identity);
        transform.GetChild(0).gameObject.SetActive(true);
        canRoll = false;
        rollTimer = rollCd;
        yield return null;
    }
    public void Shoot(bool hit)
    {
        if(!reloading && canShoot)
        {
            if (ammo > 0)
            {
                if (hit)
                {
                    Instantiate(bulletHitPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
                }
                else
                {
                    Instantiate(bulletMissedPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
                }

                audio.PlayOneShot(shootClip);
                StopCoroutine(ShakeCamera(shakeIntensity, shakeTime));
                StartCoroutine(ShakeCamera(shakeIntensity, shakeTime));
                ammo--;
               // GameObject particle = Instantiate(particlesPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
                //particle.transform.localScale = particle.transform.localScale * 0.2f;

                canShoot = false;
                shootTimer = rateOfFire;
                ammoUI.text = ammo + "/" + ammoMax;
            }
            else
            {
                audio.PlayOneShot(noAmmoClip);
            }
        }
        
    }

    public IEnumerator ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(time);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        yield return null;
    }
    void Reload()
    {
        reloadCanvas.SetActive(true);
        Time.timeScale = 0.2f;
    }

    public IEnumerator Reload(int bullet)
    {
        if (ammo < ammoMax)
        {
            reloading = true;
            audio.PlayOneShot(reloadClip);
            
            float elapsedTime = 0;
            reloadBar.transform.parent.gameObject.SetActive(true);
            while (elapsedTime < reloadTime)
            {
                reloadBar.value = elapsedTime / reloadTime;
                reloadBar.transform.parent.rotation = Quaternion.Euler(mainCamera.transform.rotation.x, mainCamera.transform.rotation.y + 90, mainCamera.transform.rotation.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            reloadBar.transform.parent.gameObject.SetActive(false);

            reloading = false;
            ammo += bullet;
            if (ammo > ammoMax)
            {
                ammo = ammoMax;
            }
            ammoUI.text = ammo + "/" + ammoMax;
        }

        yield return null;
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

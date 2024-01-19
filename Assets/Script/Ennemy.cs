using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ennemy : MonoBehaviour
{
    public int hp;
    Animator anim;
    public GameObject healthBarCanvas;
    public Slider healthBar;
    Camera cam;
    public GameObject particlesPrefab;
    private void Start()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
        healthBar.maxValue = hp;
        healthBar.value = hp;

        healthBarCanvas.transform.rotation = Quaternion.Euler(cam.transform.rotation.x, cam.transform.rotation.y+90, cam.transform.rotation.z);
    }
    private void Update()
    {
        
       // healthBarCanvas.transform.LookAt(cam.transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            GameObject particle = Instantiate(particlesPrefab, other.transform.position, other.transform.rotation);
            particle.transform.localScale = particle.transform.localScale * 0.2f;
            Destroy(other.gameObject);
            //int damage = other.GetComponent<Bullet>().baseDamage * RythmGameManager.instance.currentMultiplier;
            
            hp -= 1;
            healthBar.value = hp;
            anim.SetTrigger("Hit");
            if (hp<=0)
            {
                anim.SetTrigger("IsDead");
            }
        }
    }
}

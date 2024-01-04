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
    private void Start()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
        healthBar.maxValue = hp;
        healthBar.value = hp;
    }
    private void Update()
    {
        
        healthBarCanvas.transform.LookAt(cam.transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            Destroy(other);
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

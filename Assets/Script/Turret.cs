using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public GameObject canon;
    public float rateOfFire;
    public bool autoTarget;
    GameObject player;
    void Start()
    {
        StartCoroutine(AutoShoot());
        player = GameObject.FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(autoTarget)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
    }

    IEnumerator AutoShoot()
    {
        Instantiate(bullet,canon.transform.position, canon.transform.rotation);
        yield return new WaitForSeconds(rateOfFire);
        StartCoroutine(AutoShoot());
    }
}

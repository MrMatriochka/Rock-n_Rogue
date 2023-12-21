using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int baseDamage = 1;
    void Start()
    {
        
    }

    public float speed;
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if(!gameObject.GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }
}

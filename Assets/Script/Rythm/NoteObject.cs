using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    public float beatTempo;
    private void Update()
    {
        transform.position -= new Vector3(beatTempo * Time.deltaTime * (1/Time.timeScale), 0, 0);
        if (Input.GetButtonDown("Fire1"))
        {
            if (canBePressed)
            {
                RythmGameManager.instance.NoteHit();
                if (transform.localPosition.x < -460 && transform.localPosition.x > -455)
                {
                    RythmGameManager.instance.PerfectHit();
                }

                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HeartUI"))
        {
            canBePressed = true;
            collision.GetComponent<HeartController>().noteDetector = true;
        }
        if (collision.CompareTag("MissedUI"))
        {
            //RythmGameManager.instance.NoteMissed();
            Destroy(gameObject);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("HeartUI"))
        {
            canBePressed = false;

            collision.GetComponent<HeartController>().noteDetector = false;
        }
    }
}

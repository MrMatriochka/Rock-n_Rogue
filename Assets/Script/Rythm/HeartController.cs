using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    public bool noteDetector;
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!noteDetector)
            {
                RythmGameManager.instance.NoteMissed();
            }
        }
    }
}

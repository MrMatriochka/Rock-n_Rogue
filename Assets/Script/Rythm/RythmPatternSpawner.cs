using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmPatternSpawner : MonoBehaviour
{
    public GameObject pattern;

    private void OnEnable()
    {
        GameObject lastNote = Instantiate(pattern, transform);
    }
}

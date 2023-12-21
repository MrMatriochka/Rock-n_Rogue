using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantBeat : MonoBehaviour
{
    public double bpm;

    double nextTick = 0.0F; // The next tick in dspTime
    double sampleRate = 0.0F;
    bool ticked = false;
    public AudioSource tickSound;
    public float timerReset;
    public float timer;

    Animator anim;
    public GameObject heart;

    public bool goodTiming;
    void Start()
    {
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;

        nextTick = startTick + (60.0 / bpm);
        timer = timerReset;

        anim = heart.GetComponent<Animator>();


    }

    //private void Update()
    //{
    //    timer =- Time.deltaTime;

    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        if (timer >=0)
    //        {
    //            print("oui");
    //        }
    //        else
    //        {
    //            print("non");
    //        }

    //    }
    //}

    void LateUpdate()
    {
        timer -= Time.deltaTime;
        //print(goodTiming);
        if (timer >= 0)
        {
            goodTiming = true;
        }
        else
        {
            goodTiming = false;
        }


        if (!ticked && nextTick >= AudioSettings.dspTime)
        {
            ticked = true;
            OnTick();
            anim.Play("Base Layer.BeatTick");

        }
    }

    // Just an example OnTick here
    void OnTick()
    {
        //Debug.Log("Tick");
        tickSound.Play();
        timer = timerReset;
        // GetComponent<AudioSource>().Play();
    }

    void FixedUpdate()
    {
        double timePerTick = 60.0f / bpm;
        double dspTime = AudioSettings.dspTime;

        while (dspTime >= nextTick)
        {
            ticked = false;
            nextTick += timePerTick;
        }

    }
}

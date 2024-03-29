using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RythmGameManager : MonoBehaviour
{
    public static RythmGameManager instance;
    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;
    public PlayerController player;
    public TMP_Text multiplierText;
    public Slider multiplierSlider;
    void Start()
    {
        instance = this;

        currentMultiplier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void NoteHit()
    {
        print("hit");
        player.Shoot(true);
        //player.Reload(1);
        if (currentMultiplier-1<multiplierThresholds.Length)
        {
            multiplierTracker++;
            multiplierSlider.value = multiplierTracker;
            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
                multiplierText.text = "X " + currentMultiplier;
                multiplierSlider.maxValue = multiplierThresholds[currentMultiplier - 1];
            }
        }
        
    }

    public void PerfectHit()
    {
        print("perfect");
    }
    public void NoteMissed()
    {
        print("missed");
        player.Shoot(false);
        multiplierTracker = 0;
        currentMultiplier = 1;
        multiplierText.text = "X " + currentMultiplier;
        multiplierSlider.value = multiplierTracker;
        multiplierSlider.maxValue = multiplierThresholds[currentMultiplier - 1];
    }
}

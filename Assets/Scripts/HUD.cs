﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider distanceSlider;

    public Image distanceSliderFill;

    public Text distanceLabel;

    public Text levelText;

    [HideInInspector]
    public float distanceMeter = 0;

    [HideInInspector]
    public float distanceMeterTotal = 0;

    [HideInInspector]
    public float distanceGoal = 10;

    public Color distanceMeterColor {
        set {
            distanceSliderFill.color = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceLabel.text = distanceMeterTotal.ToString("N2") + " m";
        distanceSlider.minValue = 0;
        distanceSlider.maxValue = 1;
        distanceSlider.value = distanceMeter / distanceGoal;
    }
}

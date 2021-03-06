﻿using System;
using UnityEngine;

[ExecuteInEditMode]
public class IndicatorLamp : MonoBehaviour
{

    public Material OnMaterial, OffMaterial;
    public bool IsOn;
    bool wasOn;
    Renderer rend;
    new Light light;
    // Use this for initialization
    void Start()
    {
        IsOn = false;
        wasOn = true;
        rend = GetComponent<Renderer>();
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OnMaterial != null && OffMaterial != null)
        {
            if(IsOn != wasOn)
            {
                var mat = IsOn ? OnMaterial : OffMaterial;
                rend.material = mat;
                light.color = mat.color;
            }

            wasOn = IsOn;
        }
    }

    public void TurnOn()
    {
        this.IsOn = true;
    }

    public void TurnOff()
    {
        this.IsOn = false;
    }

    public void Toggle()
    {
        this.IsOn = !this.IsOn;
    }
}

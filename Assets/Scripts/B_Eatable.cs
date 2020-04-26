﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Material))]

public class B_Eatable : MonoBehaviour
{
    public Material b_Color;
    public Renderer rend;
    public bool isColor;
    public bool isMultiColor;
    public bool isSpeed;
    public bool isShield;
    ParticleSystem[] powerUpParticles;
    Transform b_Tran;

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        
        if (!b_Color)
        {
            if (gameObject.tag == "Player") b_Color = Resources.Load("Materials/Player") as Material;
            
        }
        if (isColor) powerUpParticles[0].Play();
        if (isMultiColor) powerUpParticles[1].Play();
        if (isSpeed) powerUpParticles[2].Play();
        if (isShield) powerUpParticles[3].Play();
    }
}

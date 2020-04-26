using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Material))]

public class B_Eatable : MonoBehaviour
{
    public enum EatableType { color, multiColor, Speed, Shield, tierOne, tierTwo, tierThree};
    BoxCollider b_collider;
    public Vector3 b_size;
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
        b_collider = GetComponent<BoxCollider>();
        b_size = b_collider.bounds.size;
        print(b_size + " " + gameObject.name);
        if (!rend)
        {
            rend = gameObject.GetComponent<Renderer>();
            if (!rend) GetComponentInChildren<Renderer>();
        }
        if (!b_Color)
        {
            if (gameObject.tag == "Player") b_Color = Resources.Load("Materials/Player") as Material;
            
        }
        if (isColor) powerUpParticles[0].Play();
        if (isMultiColor) powerUpParticles[1].Play();
        if (isSpeed) powerUpParticles[2].Play();
        if (isShield) powerUpParticles[3].Play();
    }
    private void Update()
    {
        if (gameObject.GetComponent<P_Movement>()) { b_size = b_collider.bounds.size;
            if (Input.GetKeyDown(KeyCode.Space)) print(b_size + " " + gameObject.name);
                    }
    }
}

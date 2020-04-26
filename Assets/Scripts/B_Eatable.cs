using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Material))]

public class B_Eatable : MonoBehaviour
{
    public enum EatableType { color, multiColor, Speed, Shield, tierOne, tierTwo, tierThree, Test};
    public EatableType e_Type;
    BoxCollider b_collider;
    public Vector3 b_size;
    public Material b_Color;
    public Renderer rend;

    ParticleSystem[] powerUpParticles;

    public int points;

    Transform b_Tran;

    void Start()
    {
        b_collider = GetComponent<BoxCollider>();
        b_size = b_collider.bounds.size;
        
        if (!rend)
        {
            rend = gameObject.GetComponent<Renderer>();
            if (!rend) GetComponentInChildren<Renderer>();
        }

        switch (e_Type)
        {
            case EatableType.color:
             //   powerUpParticles[0].Play();
                break;
            case EatableType.multiColor:
               // powerUpParticles[1].Play();
                break;
            case EatableType.Speed:
                //powerUpParticles[2].Play();
                break;
            case EatableType.Shield:
                //powerUpParticles[3].Play();
                break;
            case EatableType.tierOne:
                points = 1;
                break;
            case EatableType.tierTwo:
                points = 5;
                break;
            case EatableType.tierThree:
                points = 10;
                break;
            case EatableType.Test:
                rend.material.color = Color.blue;
                break;
            default:
                break;
        }
    
    }
    private void Update()
    {
        if (gameObject.GetComponent<P_Movement>()) b_size = b_collider.bounds.size;

    }
}

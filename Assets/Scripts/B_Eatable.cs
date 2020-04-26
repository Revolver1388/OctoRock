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
     Color[] colours = { Color.red, Color.blue, Color.green, Color.yellow, Color.white, Color.white};
    
    ParticleSystem[] powerUpParticles;

    BoxCollider player;
    public int points;
    int c_Choice;
    Transform b_Tran;

    void Start()
    {
        b_collider = GetComponent<BoxCollider>();
        b_size = b_collider.bounds.size;
        c_Choice = Random.Range(0, colours.Length);
        player = FindObjectOfType<P_Movement>().gameObject.GetComponent<BoxCollider>();

        if (!rend)
        {
            rend = gameObject.GetComponent<Renderer>();
            //if (!rend) GetComponentInChildren<Renderer>();
        }
        colours[5] = rend.material.color;

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
               
                break;
            default:
                break;
        }
    
    }
    private void Update()
    {
        

        if (GetComponent<BoxCollider>().bounds.size.y < player.bounds.size.y ) rend.material.color = colours[c_Choice];
        else
            rend.material.color = colours[5];

    }
}

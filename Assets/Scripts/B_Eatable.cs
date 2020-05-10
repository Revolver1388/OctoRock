using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Material))]

public class B_Eatable : MonoBehaviour
{
    public enum EatableType { color, multiColor, Speed, Shield, tierOne, tierTwo, tierThree};
    public EatableType e_Type;
    BoxCollider b_collider;
    public Material b_Color;
    public Renderer rend;
    Renderer[] children;
     Color[] colours = { Color.red, Color.blue, Color.green, Color.yellow, Color.white, Color.magenta};
    
    ParticleSystem[] powerUpParticles;

    GameObject player;
    public float points;
    int c_Choice;
    public bool isMulti;
    public Color p_Color;
    void Start()
    {
  

        if (!rend)
            rend = gameObject.GetComponent<Renderer>();
        player = FindObjectOfType<P_Movement>().gameObject;
        b_collider = GetComponent<BoxCollider>();
        colours[5] = rend.material.color;

        switch (e_Type)
        {
            case EatableType.color:
                StartCoroutine(ChangeColor(5, 10));
                rend.material.color = colours[c_Choice];
                children = GetComponentsInChildren<Renderer>();
                foreach (var child in children)
                {
                    child.material.color = colours[c_Choice];
                }
             //   powerUpParticles[0].Play();
                break;
            case EatableType.multiColor:        

                children = GetComponentsInChildren<Renderer>();
                rend.material.color = colours[0];
                children[1].material.color = colours[1];
                children[2].material.color = colours[2];
                children[3].material.color = colours[3];
                children[0].material.color = colours[4];
                StartCoroutine(ChangeColor(.04f,0.07f));
                // powerUpParticles[1].Play();
                break;
            case EatableType.Speed:
                //powerUpParticles[2].Play();
                break;
            case EatableType.Shield:
                //powerUpParticles[3].Play();
                break;
            case EatableType.tierOne:
                c_Choice = Random.Range(0, colours.Length);
                points = .5f;
                break;
            case EatableType.tierTwo:
                c_Choice = Random.Range(0, colours.Length);
                points = 1f;
                break;
            case EatableType.tierThree:
                c_Choice = Random.Range(0, colours.Length);
                points = 1.5f;
                break;
            default:
                break;
        }
    
    }
    IEnumerator ChangeColor(float i, float j)
    {
        c_Choice = Random.Range(0, colours.Length);
        yield return new WaitForSeconds(Random.Range(i,j));
        StartCoroutine(ChangeColor(i,j));
    }
    private void Update()
    {                
        switch (e_Type)
        {
            case EatableType.color:               
                rend.material.color = colours[c_Choice];
                children = GetComponentsInChildren<Renderer>();
                foreach (var child in children)
                {
                    child.material.color = colours[c_Choice];
                }
    
                    break;
            case EatableType.multiColor:
                rend.material.color = colours[c_Choice];
                break;
            case EatableType.Speed:
                break;
            case EatableType.Shield:
                break;
            case EatableType.tierOne:
                if (transform.localScale.y < player.transform.localScale.y)
                        rend.material.color = colours[c_Choice];
                else if (transform.localScale.y > player.transform.localScale.y)
                    rend.material.color = colours[5];
                //else if (b_collider.bounds.size.y < player.bounds.size.y / 2 && player.bounds.size.y > b_collider.size.y * 4)
                //    rend.material.color = colours[5];
                break;
            case EatableType.tierTwo:
                if (transform.localScale.y < player.transform.localScale.y)
                    rend.material.color = colours[c_Choice];
                else if (transform.localScale.y > player.transform.localScale.y)
                    rend.material.color = colours[5];
                //else if (b_collider.bounds.size.y < player.bounds.size.y / 2 && player.bounds.size.y > b_collider.size.y * 4)
                //    rend.material.color = colours[5];
                break;
            case EatableType.tierThree:
                if (transform.localScale.y < player.transform.localScale.y)
                    rend.material.color = colours[c_Choice];
                else if (transform.localScale.y > player.transform.localScale.y)
                    rend.material.color = colours[5];
                //else if (b_collider.bounds.size.y < player.bounds.size.y / 2 && player.bounds.size.y > b_collider.size.y * 4)
                //    rend.material.color = colours[5];
                break;
            default:
                break;
        }
    }
}

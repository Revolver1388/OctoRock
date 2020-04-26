using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Material))]

public class B_Eatable : MonoBehaviour
{
    public Material b_Color;
    public Renderer rend;
    Transform b_Tran;

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        if (!b_Color)
        {
            if (gameObject.tag == "Player") b_Color = Resources.Load("Materials/Player") as Material;
            
        }
    }
}

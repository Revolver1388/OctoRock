using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// stops things from falling through the floor

public class GroundHelper : MonoBehaviour
{
    public float maxDistFromFloor = 0;
    public LayerMask whatIsFloor;
    public float forceMult = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)
        {
            transform.position += transform.up * forceMult;
        }
        
        if (Physics.Raycast(transform.position, -transform.up, maxDistFromFloor, whatIsFloor))
        {
            Debug.Log("Change direction to avoid obstacle");
            transform.position += transform.up * forceMult;

        }
        

    }
}

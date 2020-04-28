using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Hop : MonoBehaviour
{
    public float jumpForce = 0f;
    private Rigidbody rbody;
    public float maxDistFromFloor = 0;
    public LayerMask whatIsFloor;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, -transform.up, maxDistFromFloor, whatIsFloor))
        {
            //Debug.Log("HOP!");
            rbody.AddForce(transform.up * jumpForce);
        }


    }
}

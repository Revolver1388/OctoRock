using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AvoidObstacles : MonoBehaviour
{
    public float moveForce = 0f;
    private Rigidbody rbody;
    public Vector3 moveDir;
    public LayerMask whatIsObstacle;
    public float maxDistFromObstacle;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        moveDir = ChooseDirection();
        transform.rotation = Quaternion.LookRotation(moveDir);
    }

    // Update is called once per frame
    void Update()
    {
        rbody.AddForce(moveDir * moveForce);


        if (Physics.Raycast(transform.position, transform.forward, maxDistFromObstacle, whatIsObstacle))
        {
            Debug.Log("Change direction to avoid obstacle");
            moveDir = ChooseDirection();
            transform.rotation = Quaternion.LookRotation(moveDir);
        }

    }

    Vector3 ChooseDirection()
    {
        System.Random ran = new System.Random();
        int i = ran.Next(0, 7);
        Vector3 temp = new Vector3();



        if (i == 0)
        {
            temp = transform.forward;
        }
        else if (i == 1)
        {
            temp = -transform.forward;
        }
        else if (i == 2)
        {
            temp = transform.right;
        }
        else if (i == 3)
        {
            temp = -transform.right;
        }
        else if (i == 4)
        {
            temp = -transform.forward + transform.right;
        }
        else if (i == 5)
        {
            temp = transform.right + transform.forward;
        }
        else if (i == 6)
        {
            temp = -transform.right + transform.forward;
        }
        else if (i == 7)
        {
            temp = -transform.right + -transform.forward;
        }

        return temp;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Movement : MonoBehaviour
{
    public float jumpForce = 0f;
    private Rigidbody rbody;
    public float maxDistFromFloor = 0;
    public LayerMask whatIsFloor;
    public float moveForce = 0f;
    public Vector3 moveDir;
    public LayerMask whatIsObstacle;
    public float maxDistFromObstacle;
    public int changeDirectionTimer = 3;
    private bool isCounting;
    public bool variableSpeed;
    public int maxSpeed;
    private float randomDirMult;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        moveDir = ChooseDirection();
        transform.rotation = Quaternion.LookRotation(moveDir);
        isCounting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, -transform.up, maxDistFromFloor, whatIsFloor))
        {
            //Debug.Log("HOP!");
            rbody.AddForce(transform.up * jumpForce);            
        }
        if (Physics.Raycast(transform.position, transform.forward, maxDistFromObstacle, whatIsObstacle))
        {
            Debug.Log("Change direction to avoid obstacle");
            moveDir = moveDir * -1;
            transform.rotation = Quaternion.LookRotation(moveDir);
            rbody.velocity = moveDir * moveForce;
        }


        if (isCounting == false)
        {
            if (variableSpeed)
            {
                System.Random ran = new System.Random();
                int i = ran.Next(1, maxSpeed);
                moveForce = i;
            }

            StartCoroutine(ChangeDirectionTimerHelper());
            Debug.Log("Changing direction after wait");
            rbody.velocity = moveDir * moveForce;
        }

        transform.rotation = Quaternion.LookRotation(moveDir);
    }

    private IEnumerator ChangeDirectionTimerHelper()
    {
        isCounting = true;
        yield return new WaitForSeconds(changeDirectionTimer);
        moveDir = ChooseDirection();
        isCounting = false;
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

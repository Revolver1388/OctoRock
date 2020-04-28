using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficBehavior : MonoBehaviour
{
    public float moveForce = 0f;
    public Vector3 moveDir;
    public LayerMask whatIsObstacle;
    public float maxDistFromObstacle;
    public string turnDirection;
    public string startDirection;
    public LayerMask whatIsVehicle;
    private int recentTurns = 0;



    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.LookRotation(moveDir);
        if (startDirection == "forward")
        {
            moveDir = transform.forward;
        }
        else if (startDirection == "right")
        {
            moveDir = transform.right;
        }
        else if (startDirection == "left")
        {
            moveDir = -transform.right;
        }
        else if (startDirection == "backward")
        {
            moveDir = -transform.forward;
        }


    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDir * Time.deltaTime * moveForce;


        if (Physics.Raycast(transform.position, moveDir, maxDistFromObstacle, whatIsObstacle))
        {
            //Debug.Log("Raycast trigger");
            if (turnDirection == "left")
            {
                moveDir = -transform.right;
                recentTurns++;
                if (recentTurns > 3)
                {
                    recentTurns = 0;
                    StartCoroutine(TurnWait());
                }
            }
            else if (turnDirection == "right")
            {
                moveDir = transform.right;
                recentTurns++;
                if (recentTurns > 3)
                {
                    recentTurns = 0;
                    StartCoroutine(TurnWait());
                }
            }
            transform.rotation = Quaternion.LookRotation(moveDir);
        }

        if (Physics.Raycast(transform.position, moveDir, maxDistFromObstacle, whatIsVehicle))
        {
            StartCoroutine(SlowDownWait());
        }

    }

    private IEnumerator SlowDownWait()
    {
        float tempfloat = moveForce;
        moveForce = 0;
        yield return new WaitForSeconds(2);
        moveForce = tempfloat;
    }

    private IEnumerator TurnWait()
    {
        string temp = turnDirection;
        turnDirection = "";
        yield return new WaitForSeconds(2);
        turnDirection = temp;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class P_Movement : MonoBehaviour
{

    public enum WalkState { KeyCrawl, WASD, TPerson, Controller}

    public WalkState thisState;
    Animator anim;
    Rigidbody rb;
    [SerializeField] GameObject[] arms;
    [SerializeField] GameObject[] f_Arms;
    [Range(1,20)]
    [SerializeField] float armSpeed = 3;
    Vector3 p_Input;

    [SerializeField] float maxReach;
    [SerializeField] float minReach;
    [Range(0.3f, 10)]
    [SerializeField] float p_MoveSpeed = 8;
    BoxCollider b_collider;
    [SerializeField] bool shoot;
    // Start is called before the first frame update    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        b_collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (thisState)
        {
            case WalkState.KeyCrawl:
                foreach (var arm in arms)
                {
                    arm.SetActive(true);
                }
                foreach (var arm in f_Arms)
                {
                    arm.SetActive(true);
                }
                if (Input.GetKeyDown(KeyCode.Space)) shoot = true;
                else if (Input.GetKeyUp(KeyCode.Space)) shoot = false;

                if (shoot)
                {
                    p_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                    rb.MovePosition(transform.position + p_Input * p_MoveSpeed * Time.deltaTime);
                }
                else if (!shoot)
                {
                    #region Moving the Arms
                    //Left1
                    //if (Input.GetKey(KeyCode.F)) { arms[3].transform.position += transform.forward * 1 * Time.deltaTime; }
                    if (Input.GetMouseButton(0)) { MouseFunctions(f_Arms[0]); TurnOffArms(true); }
                    else if (Input.GetMouseButtonUp(0)) TurnOffArms(false);
            
                    //Left2
                    if (Input.GetKey(KeyCode.D)) { arms[2].transform.position += (transform.forward + (-transform.right / 6)) * 1 * Time.deltaTime; }
                    //Left3
                    if (Input.GetKey(KeyCode.S)) { arms[1].transform.position += (transform.forward + -transform.right / 8) * 1 * Time.deltaTime; }
                    //Left4
                    if (Input.GetKey(KeyCode.A)) { arms[0].transform.position += transform.forward * 1 * Time.deltaTime; }

                    //Right1
                    //if (Input.GetKey(KeyCode.J)) { arms[4].transform.position += transform.forward * 1 * Time.deltaTime; }
                    if (Input.GetMouseButton(1)) { MouseFunctions(f_Arms[1]); TurnOffArms(true); }
                    else if (Input.GetMouseButtonUp(1)) TurnOffArms(false);

                    //Right2
                    if (Input.GetKey(KeyCode.K)) { arms[3].transform.position += (transform.forward + transform.right / 6) * 1 * Time.deltaTime; }
                    //Right3
                    if (Input.GetKey(KeyCode.L)) { arms[4].transform.position += (transform.forward + (transform.right / 8)) * 1 * Time.deltaTime; }
                    //Right4
                    if (Input.GetKey(KeyCode.Semicolon)) { arms[5].transform.position += transform.forward * 1 * Time.deltaTime; }
                    #endregion
                }
                    break;
            case WalkState.WASD:
                foreach (var arm in arms)
                {
                    arm.SetActive(false);
                }
                foreach (var arm in f_Arms)
                {
                    arm.SetActive(false);
                }
                p_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                
                anim.SetFloat("Forward", p_Input.z);
                anim.SetFloat("LeftRight", p_Input.x);
                rb.MovePosition(transform.position + p_Input * p_MoveSpeed * Time.deltaTime);
                foreach (var arm in arms)
                {
                    arm.GetComponent<Rigidbody>().MovePosition(arm.transform.position + p_Input * p_MoveSpeed * Time.deltaTime);
                }
                break;
            case WalkState.TPerson:
                break;
            case WalkState.Controller:
                if (Input.GetKey(KeyCode.F)) { arms[3].transform.position += transform.forward * 1 * Time.deltaTime; arms[2].transform.position += (transform.forward + (-transform.right / 4)) * 1 * Time.deltaTime; }
                if (Input.GetKey(KeyCode.S)) { arms[1].transform.position += (transform.forward + -transform.right/8) * 1 * Time.deltaTime; arms[0].transform.position += transform.forward * 1 * Time.deltaTime; }
                if (Input.GetKey(KeyCode.J)) { arms[4].transform.position += transform.forward * 1 * Time.deltaTime; arms[5].transform.position += (transform.forward + transform.right/4) * 1 * Time.deltaTime; }
                if (Input.GetKey(KeyCode.L)) { arms[6].transform.position += (transform.forward + (transform.right / 8)) * 1 * Time.deltaTime; arms[7].transform.position += transform.forward * 1 * Time.deltaTime; }
               


                break;
            default:
                break;
        }
    }

    void toggleBool(bool x)
    {
        _ = !x;
    }

    RaycastHit MouseFunctions(GameObject j)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            j.transform.position = hit.point;
        }
        return hit;
    }

    void TurnOffArms(bool x)
    {
        foreach (var arm in arms)
        {
            arm.GetComponent<Rigidbody>().isKinematic = x;
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<B_Eatable>())
        {
            if (c.GetComponent<B_Eatable>().b_size.y <= GetComponent<B_Eatable>().b_size.y)
            {
                if (c.GetComponent<B_Eatable>().b_size.x <= GetComponent<B_Eatable>().b_size.x)
                {
                    if (c.GetComponent<B_Eatable>().rend.material.color == this.gameObject.GetComponent<B_Eatable>().rend.material.color)
                    {
                        //Play SFX
                        //Play Animation

                        Destroy(c.gameObject);
                        //Play CHomp Animation
                        //Play Chomp SFX
                        //Play Chomp VFX
                        print("YUMMMMM!");
                        gameObject.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                    }
                    else
                    {
                        print("Can't Eat, Wrong Color! YUK! PUKING!! SHRINKING!!!");
                        //Play Vomit Animation
                        //Play Shrinking VFX
                        //Play Shringin SFX

                        gameObject.transform.localScale += new Vector3(-0.03f, -0.03f, -0.03f);
                    }
                }
                else
                    //Bounce Player
                    //Player Can't Eat Animation
                    print("Can't Eat, To wide!");
            }
            else
                //Bounce Player
                //Player Can't Eat Animation
                print("Can't Eat, To tall!");
        }
    }
}

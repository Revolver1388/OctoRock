using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class P_Movement : MonoBehaviour
{

    public enum WalkState { KeyCrawl, WASD, TPerson, Controller}

    public WalkState thisState;
    [SerializeField] Camera cam;
    [SerializeField] float rotateSpeed;
    LevelStart lvl_Man;
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
    Renderer rend;
    Color[] colours = { Color.red, Color.blue, Color.green, Color.yellow, Color.white };
    int score;
    // Start is called before the first frame update    
    void Start()
    {
        cam = Camera.main;
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        b_collider = GetComponent<BoxCollider>();
        lvl_Man = FindObjectOfType<LevelStart>();
        StartCoroutine(ChangeColor());
    }

    // Update is called once per frame
    void Update()
    {
        if (b_collider.bounds.size.y <= .02f) lvl_Man.gameOver = true;
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
                p_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                Vector3 camF = cam.transform.TransformDirection(Vector3.forward);
                Vector3 camR = cam.transform.TransformDirection(Vector3.right);
                camF.y = 0;
                camR.y = 0;
                camF = camF.normalized;
                anim.SetFloat("Forward", p_Input.z);
                anim.SetFloat("LeftRight", p_Input.x);

                transform.position += (camF * p_Input.z + camR * p_Input.x) * p_MoveSpeed * Time.deltaTime;
                //rb.MovePosition(transform.position + p_Input * p_MoveSpeed * Time.deltaTime);
  
                foreach (var arm in arms)
                {
                    arm.SetActive(false);
                }
                foreach (var arm in f_Arms)
                {
                    arm.SetActive(false);
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

    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(Random.Range(5, 15));
        rend.material.color = colours[Random.Range(0, colours.Length)];
        StartCoroutine(ChangeColor());
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
            int x = c.GetComponent<B_Eatable>().points;

            if (c.GetComponent<B_Eatable>().b_size.y <= b_collider.bounds.size.y)
            {
                if (c.GetComponent<B_Eatable>().b_size.x <= b_collider.bounds.size.x)
                {
                    if (c.GetComponent<B_Eatable>().rend.material.color == rend.material.color)
                    {
                        score += x;
                        
                        //Play SFX
                        //Play Animation

                        Destroy(c.gameObject);
                        //Play CHomp Animation
                        //Play Chomp SFX
                        //Play Chomp VFX
                        print("YUMMMMM!");
                        print(b_collider.bounds.size + " Before");
                        
                        gameObject.transform.localScale += new Vector3(x/10,x/10,x/10);
                       
                        cam.GetComponent<PlayerCamera>().distFromPlayer += x;
                    }
                    else
                    {
                        print("Can't Eat, Wrong Color! YUK! PUKING!! SHRINKING!!!");
                        score -= c.GetComponent<B_Eatable>().points;
                        //Play Vomit Animation
                        //Play Shrinking VFX
                        //Play Shringin SFX
                        Destroy(c.gameObject);
                        print(b_collider.bounds.size + " Before");
                        gameObject.transform.localScale += new Vector3(-x/8, -x/8, -x/8);
                        cam.GetComponent<PlayerCamera>().distFromPlayer -= x;
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
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<B_Eatable>())
        print(b_collider.bounds.size + " After");
    }
}

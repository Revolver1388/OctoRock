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
    bool isMoving = false;
    AudioSource mein_Audio;
    AudioManager audio_Man;
    [SerializeField] float growthSpeed;
    Color[] colours = { Color.red, Color.blue, Color.green, Color.yellow, Color.white };
    public float score;
    int i;
    public bool isMultiColor = false;
    void Start()
    {
        i = Random.Range(0, colours.Length);
        cam = Camera.main;
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        b_collider = GetComponent<BoxCollider>();
        lvl_Man = FindObjectOfType<LevelStart>();
        audio_Man = FindObjectOfType<AudioManager>();
        mein_Audio = GetComponent<AudioSource>();
       // StartCoroutine(ChangeColor());
        rend.material.color = colours[i];
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
                p_Input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                Vector3 camF = cam.transform.TransformDirection(Vector3.forward);
                Vector3 camR = cam.transform.TransformDirection(Vector3.right);
                camF.y = 0;
                camR.y = 0;
                camF = camF.normalized;
                anim.SetFloat("Forward", p_Input.z);
                anim.SetFloat("LeftRight", p_Input.x);
                //anim.SetBool("isMoving", isMoving);
      
                transform.position += (camF * p_Input.z + camR * p_Input.x) * p_MoveSpeed * Time.deltaTime;
                //rb.MovePosition(transform.position + p_Input * p_MoveSpeed * Time.deltaTime);
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) isMoving = true; 
                else if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) isMoving = false;
                
                if (isMoving && mein_Audio.isPlaying == false) mein_Audio.Play();
                if (!isMoving) mein_Audio.Stop();
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
                if (Input.GetKey(KeyCode.S)) { arms[1].transform.position += (transform.forward + -transform.right / 8) * 1 * Time.deltaTime; arms[0].transform.position += transform.forward * 1 * Time.deltaTime; }
                if (Input.GetKey(KeyCode.J)) { arms[4].transform.position += transform.forward * 1 * Time.deltaTime; arms[5].transform.position += (transform.forward + transform.right / 4) * 1 * Time.deltaTime; }
                if (Input.GetKey(KeyCode.L)) { arms[6].transform.position += (transform.forward + (transform.right / 8)) * 1 * Time.deltaTime; arms[7].transform.position += transform.forward * 1 * Time.deltaTime; }
                break;
            default:
                break;
        }
    }

    IEnumerator ChangeColor(float i, float j)
    {
        yield return new WaitForSeconds(Random.Range(i,j));
        rend.material.color = colours[Random.Range(0, colours.Length)];
        if(isMultiColor)
        StartCoroutine(ChangeColor(i,j));
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

    IEnumerator MultiColored()
    {
        yield return new WaitForSeconds(10);
        isMultiColor = false;
    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<B_Eatable>())
        {
            float x = c.GetComponent<B_Eatable>().points;
            if (c.GetComponent<B_Eatable>().e_Type == B_Eatable.EatableType.color)
            {
                rend.material.color = c.GetComponent<B_Eatable>().rend.material.color;
                //Destroy(c.gameObject);
            }
            else if (c.GetComponent<B_Eatable>().e_Type == B_Eatable.EatableType.multiColor)
            {
                isMultiColor = true;
                StartCoroutine(MultiColored());
                StartCoroutine(ChangeColor(.04f, .07f));

            }
            else if (c.GetComponent<B_Eatable>().e_Type == B_Eatable.EatableType.Shield)
            {

            }
            else if (c.GetComponent<B_Eatable>().e_Type == B_Eatable.EatableType.Speed)
            {

            }
            else
            {
                if (!isMultiColor)
                {
                    if (c.GetComponent<B_Eatable>().b_size.y <= b_collider.bounds.size.y && b_collider.bounds.size.y <= c.GetComponent<B_Eatable>().b_size.y * 10)
                    {
                        //if (c.GetComponent<B_Eatable>().b_size.x <= b_collider.bounds.size.x)
                        //{
                        if (c.GetComponent<B_Eatable>().rend.material.color == rend.material.color)
                        {
                            score += x * 10;

                            //Play SFX
                            //Play Animation

                            Destroy(c.gameObject);
                            //Play CHomp Animation
                            //Play Chomp SFX
                            //Play Chomp VFX
                            print("YUMMMMM!");
                            print(b_collider.bounds.size + " Before");
                            audio_Man.PlayOneShotByIndex(Random.Range(0, audio_Man.eat.Length), audio_Man.sfxSource);
                            gameObject.transform.localScale += new Vector3(x, x, x);
                            //gameObject.transform.localScale += new Vector3(x, x, x);

                        }
                        else if (c.GetComponent<B_Eatable>().rend.material.color != rend.material.color)
                        {
                            print("Can't Eat, Wrong Color! YUK! PUKING!! SHRINKING!!!");
                            score -= c.GetComponent<B_Eatable>().points;
                            //Play Vomit Animation
                            //Play Shrinking VFX
                            //Play Shringin SFX
                            Destroy(c.gameObject);
                            print(b_collider.bounds.size + " Before");
                            gameObject.transform.localScale += new Vector3(-x, -x, -x);
                        }
                    }
                }
                else
                {
                    score += x * 10;

                    //Play SFX
                    //Play Animation

                    Destroy(c.gameObject);
                    //Play CHomp Animation
                    //Play Chomp SFX
                    //Play Chomp VFX
                    print("YUMMMMM!");
                    print(b_collider.bounds.size + " Before");
                    audio_Man.PlayOneShotByIndex(Random.Range(0, audio_Man.eat.Length), audio_Man.sfxSource);
                    gameObject.transform.localScale += new Vector3(x, x, x);
                    //gameObject.transform.localScale += new Vector3(x, x, x);
                }
                //else
                //    //Bounce Player
                //    //Player Can't Eat Animation
                //    print("Can't Eat, To wide!");
                //}
                //else
                //    //Bounce Player
                //    //Player Can't Eat Animation
                //    print("Can't Eat, To tall!");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<B_Eatable>())
        print(b_collider.bounds.size + " After");
    }
}

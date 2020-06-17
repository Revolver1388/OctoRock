﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody))]

public class P_Movement : MonoBehaviour
{

    public enum WalkState { WASD }
    public WalkState thisState;

    RaycastHit ground;
    [SerializeField] float _gHeight;
    float _gCheckDist;
    public LayerMask IgnoreMask;
    LevelStart lvl_Man;
    [SerializeField] float rotateSpeed;
    [SerializeField] Camera cam;
    Vector3 p_Input;
    float p_MoveSpeed = 8;
    bool isMoving = false;
    bool canMove = true;
    public bool isGrounded;
    [SerializeField] Animator anim;
    [SerializeField] Renderer rend;
    [SerializeField] AudioSource mein_Audio;
    AudioManager audio_Man;
    public static P_Movement _instance;
    [SerializeField] Color[] colours = { Color.red, Color.blue, Color.green, Color.yellow, Color.white };
    int _colorInt = 0;
    public bool isMultiColor = false;
    int i;
    public float score;


 
    void Start()
    {
        if (!_instance) _instance = this;
        else
            Destroy(_instance);
        i = Random.Range(0, 4);
        cam = Camera.main;
        lvl_Man = FindObjectOfType<LevelStart>();
        audio_Man = FindObjectOfType<AudioManager>();
        rend.material.color = colours[i];
        thisState = WalkState.WASD;
    }

    // Update is called once per frame
    void Update()
    {              
       // _gCheckDist = (transform.localScale.y / 2) + 0.2f;
       // if (isGrounded) transform.position = new Vector3(transform.position.x, ground.point.y + _gHeight, transform.position.z);
        p_MoveSpeed = (transform.localScale.y / 3) + 8;//player speed dependent on size**** Adjust for balance
     
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Eat") || anim.GetCurrentAnimatorStateInfo(0).IsName("Grow")) canMove = false;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) canMove = true;
        if (transform.localScale.y <= .02f) lvl_Man.gameOver = true;
        if (!isMultiColor)
        {
            ColorWheel();
        }
        switch (thisState)
        {
            case WalkState.WASD:
                Vector3 camF = cam.transform.TransformDirection(Vector3.forward);
                Vector3 camR = cam.transform.TransformDirection(Vector3.right);
                camF.y = 0;
                camR.y = 0;
                camF.Normalize();
                camR.Normalize();
                transform.localRotation = Quaternion.LookRotation(camF, cam.transform.up);
                if (canMove)
                {
                    p_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    anim.SetBool("isMoving", isMoving);

                    transform.position += (camF * p_Input.z + camR * p_Input.x) * p_MoveSpeed * Time.deltaTime;

                    if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) isMoving = true;
                    else if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) isMoving = false;

                    if (isMoving && mein_Audio.isPlaying == false) { mein_Audio.Play(); }
                    if (!isMoving) mein_Audio.Stop();

                    //This if for testing cam
                    //transform.localScale += new Vector3(Input.GetAxis("Mouse ScrollWheel"), Input.GetAxis("Mouse ScrollWheel"), Input.GetAxis("Mouse ScrollWheel"));
                }
                break;
            default:
                break;
        }
    }
    private void FixedUpdate()
    {
        //CheckGround();

    }


    IEnumerator ChangeColor(float i, float j)
    {
        yield return new WaitForSeconds(Random.Range(i, j));
        rend.material.color = colours[Random.Range(0, colours.Length)];
        if (isMultiColor)
            StartCoroutine(ChangeColor(i, j));
    }
    void toggleBool(bool x)
    {
        x = !x;
    }

    void ColorWheel()
    {
        if (_colorInt > 4) _colorInt = 0;
        if (_colorInt <= -1) _colorInt = 4;
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            _colorInt += 1;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            _colorInt -= 1;
        rend.material.color = colours[_colorInt];
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

    IEnumerator MultiColored()
    {
        yield return new WaitForSeconds(10);
        isMultiColor = false;
    }
    //void CheckGround()
    //{
    //    if (Physics.Raycast(transform.position, -Vector3.up, out ground, _gCheckDist))
    //    {
    //        //Vector3 groundAngle = Vector3.Cross(ground.normal, Vector3.down);
    //        //Vector3 groundSlopeDirection = Vector3.Cross(groundAngle, ground.normal);
    //        isGrounded = true;
    //    }
    //    else
    //        isGrounded = false;
                
    //}
    private void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<B_Eatable>())
        {
            float x = c.GetComponent<B_Eatable>().points;
            if (c.GetComponent<B_Eatable>().e_Type == B_Eatable.EatableType.color)
            {
                //rend.material.color = c.GetComponent<B_Eatable>().rend.material.color;
                Destroy(c.gameObject);
            }
            else if (c.GetComponent<B_Eatable>().e_Type == B_Eatable.EatableType.multiColor)
            {
                isMultiColor = true;
                StartCoroutine(MultiColored());
                StartCoroutine(ChangeColor(.04f, .07f));
            }

            else
            {
                if (canMove)
                {
          
                    if (c.GetComponent<BoxCollider>().bounds.size.y > GetComponent<BoxCollider>().bounds.size.y / 6)
                    {
                        if (c.GetComponent<BoxCollider>().bounds.size.y <= GetComponent<BoxCollider>().bounds.size.y + 1)
                        {
                            if (!isMultiColor)
                            {
                                if (c.GetComponent<B_Eatable>().rend.material.color == rend.material.color)
                                {
                                    score += x * 10;
                                    anim.SetTrigger("Eating");
                                    Destroy(c.gameObject);
                                    audio_Man.PlayOneShotByIndex(Random.Range(0, audio_Man.eat.Length), audio_Man.sfxSource);
                                    transform.localScale += new Vector3(x, x, x);
                                    anim.SetTrigger("Grow");

                                }
                                else if (c.GetComponent<B_Eatable>().rend.material.color != rend.material.color)
                                {
                                    anim.SetTrigger("Eating");
                                    score -= c.GetComponent<B_Eatable>().points;
                                    c.gameObject.GetComponent<BoxCollider>().enabled = false;
                                    Destroy(c.gameObject);
                                    transform.localScale += new Vector3(-x, -x, -x);
                                }
                            }
                           else
                            {
                                score += x * 10;
                                anim.SetTrigger("Eating");
                                Destroy(c.gameObject);
                                audio_Man.PlayOneShotByIndex(Random.Range(0, audio_Man.eat.Length), audio_Man.sfxSource);
                                transform.localScale += new Vector3(x, x, x);
                                anim.SetTrigger("Grow");
                            }
                        }
                    }
                }
            }
        }
    }


    private void OnTriggerExit(Collider c)
    {

        if (c.GetComponent<B_Eatable>())
        {
            if (c.GetComponent<B_Eatable>().e_Type == B_Eatable.EatableType.color)
            {
            }
            else if (c.GetComponent<B_Eatable>().e_Type == B_Eatable.EatableType.multiColor)
            {
            }
        }
    }
}

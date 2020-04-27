//Written by: Kyle J. Ennis
//last edited 14/10/19
//last edited 14/11/19

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera/Player Camera", 0)]
[RequireComponent(typeof(Camera))]

public class PlayerCamera : MonoBehaviour
{

    #region Player
    GameObject Player;
    P_Movement pl;
    Rigidbody p_RB;
    #endregion
    #region UI and Player Choices
    [Header("Turn on/off the mouse cursor")]
    public bool lockCursor = false;
    [Header("Invert Camera Y")]
    public bool invY;
    [Header("Camera Sensitivity and rotation smoothing")]
    [Tooltip("Adjusts camera movement sensitivity.")]
    [Range(1, 10)]
    public float sensitivity = 2.5f;
    [Tooltip("Adjusts how quickly the camera moves acording to user input.")]
    [Range(0, 1)]
    public float rotationsmoothTime = 0.202f;
    #endregion
    #region Camera Distance and Orbit Management
    [Header("Distance from Player")]
    [Range(1, 100)]
    [SerializeField] public float distFromPlayer = 1;
    float YAxis;
    float YDistSpeed = 1;
    float camDistMax = 100;
    float camDistMin = 0.2f;
    [SerializeField] Vector2 pitchMinMax;
    Vector3 currentRotation;
    Vector3 smoothingVelocity;
    float yaw;
    float pitch;
    #endregion
    #region CameraRayCasts
    Camera main;
    GameObject aimer;
    // int p_LayerMask = 1 << 9;
    public LayerMask IgnoreMask;
    RaycastHit hit;
    float wallCheckSmoothing = 20;
    bool enclosed = false;
    [SerializeField] float cameraOffsetX;
    [SerializeField] float cameraOffsetY;
    float camDist;

    #endregion
    #region Field of View and Zoom Function Smoothing
    float i_FOV = 70;
    float c_FOV;
    float enclosedCam_FOV = 75;
    #endregion

    enum CameraType { Orbit }
   


    private void Awake()
    {
        if (!Player)
            Player = FindObjectOfType<P_Movement>().gameObject;
        if (!pl) pl = FindObjectOfType<P_Movement>();
        if (!aimer)
            aimer = GameObject.FindGameObjectWithTag("CameraAimer");
        if (!main)
            main = Camera.main;
        if (!p_RB)
            p_RB = Player.GetComponent<Rigidbody>();
    }
    void Start()
    {
       
        currentRotation = Vector3.SmoothDamp(currentRotation, Player.transform.eulerAngles, ref smoothingVelocity, rotationsmoothTime);
        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (main != null)
        {
            c_FOV = main.fieldOfView;
        }
        distFromPlayer = UpDownCam(invY);
        //camDist = CameraRaycast(distFromPlayer);
        if (Player.GetComponent<BoxCollider>().bounds.size.y < 0.5f) distFromPlayer = 10;
        else if (Player.GetComponent<BoxCollider>().bounds.size.y >= 0.5f && Player.GetComponent<BoxCollider>().bounds.size.y < 1) distFromPlayer = 20;
        else if (Player.GetComponent<BoxCollider>().bounds.size.y >= 1 && Player.GetComponent<BoxCollider>().bounds.size.y < 1.5) distFromPlayer = 30;
        else if (Player.GetComponent<BoxCollider>().bounds.size.y >= 1.5f && Player.GetComponent<BoxCollider>().bounds.size.y < 2) distFromPlayer = 40;
        else if (Player.GetComponent<BoxCollider>().bounds.size.y >= 2f && Player.GetComponent<BoxCollider>().bounds.size.y < 2.5f) distFromPlayer = 50;
        else if (Player.GetComponent<BoxCollider>().bounds.size.y >= 2.5f && Player.GetComponent<BoxCollider>().bounds.size.y < 3) distFromPlayer = 60;
        else if (Player.GetComponent<BoxCollider>().bounds.size.y >= 3 && Player.GetComponent<BoxCollider>().bounds.size.y < 3.5f) distFromPlayer = 70;
        else if (Player.GetComponent<BoxCollider>().bounds.size.y >= 3.5f && Player.GetComponent<BoxCollider>().bounds.size.y < 4) distFromPlayer = 80;
        else if (Player.GetComponent<BoxCollider>().bounds.size.y >= 4f && Player.GetComponent<BoxCollider>().bounds.size.y < 4.5f) distFromPlayer = 90;
        else if (Player.GetComponent<BoxCollider>().bounds.size.y >= 4.5f) distFromPlayer = 100;



        camDist = distFromPlayer;
        //camDist = Player.GetComponent<BoxCollider>().bounds.size.y + 10;
    }
    private void LateUpdate()
    {
        CamMovement3D();
    }

    //Basic Camera Movements, Orbit around player and joystick control, as well as player offset
    void CamMovement3D()
    {
        YAxis = Input.GetAxis("MouseY") * sensitivity;
        pitch = (invY) ? pitch += YAxis : pitch -= YAxis;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref smoothingVelocity, rotationsmoothTime);
        yaw += Input.GetAxis("MouseX") * sensitivity;
        transform.eulerAngles = currentRotation;
        transform.position = Player.transform.position - (transform.forward - (transform.right * cameraOffsetX) + (transform.up * cameraOffsetY)) * camDist;
    }
    #region CameraUtilities

    //changes the distance of the cam based on the angle of the camera
    float UpDownCam(bool x)
    {
        if (distFromPlayer < camDistMin)
            distFromPlayer = camDistMin;
        if (distFromPlayer > camDistMax)
            distFromPlayer = camDistMax;

        if (x)
        {
            if (distFromPlayer <= camDistMax && distFromPlayer >= camDistMin)
                distFromPlayer += YAxis * YDistSpeed * Time.deltaTime;
            return distFromPlayer;
        }
        else
            if (distFromPlayer <= camDistMax && distFromPlayer >= camDistMin)
            distFromPlayer -= YAxis * YDistSpeed * Time.deltaTime;
        return distFromPlayer;
    }

    //Checks cameras distance from the player and adjusts accordingly
    float CameraRaycast(float x)
    {
        if (Physics.Raycast(aimer.transform.position, main.transform.position - aimer.transform.position, out hit, x, IgnoreMask))
            return hit.distance - 0.2f;
        else
            return x;
    }

    //needs to be tied to menu system for adjustment by player
    public void switchInverseY()
    {
        invY = !invY;
    }

    //Zooms the camera out when in tight enclosures for a better view
    void EnclosedCameraFunctions(bool w, bool x, bool y, bool z)
    {
        if (w && x || y && z)
            enclosed = true;
        else if (!w && !x || !y && !z)
        {
            enclosed = false;
        }
        CameraZooms(enclosed, wallCheckSmoothing, enclosedCam_FOV);
    }

    //set this up for all zooms, cinematic and what not 
    void CameraZooms(bool x, float smoothing, float t_goal)
    {
        switch (x)
        {
            case true:
                if (c_FOV < t_goal)
                    main.fieldOfView += smoothing * Time.deltaTime;
                else
                    main.fieldOfView = t_goal;
                break;
            case false:
                if (c_FOV > i_FOV)
                    main.fieldOfView -= smoothing * Time.deltaTime;
                else
                    main.fieldOfView = i_FOV;
                break;
        }
    }
    #endregion
}


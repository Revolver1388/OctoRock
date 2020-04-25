using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStart : MonoBehaviour
{
    [SerializeField] Image fadeIn;
    [SerializeField] Vector3 fadeIn_Rotation;
    [SerializeField] float fadeIn_RotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        fadeIn.CrossFadeAlpha(0, 2, false);
    }

    // Update is called once per frame
    void Update()
    {
        fadeIn.transform.Rotate(fadeIn_Rotation * fadeIn_RotationSpeed * Time.deltaTime);
        
    }
}

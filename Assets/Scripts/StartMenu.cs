using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject[] Buttons;
    [SerializeField] Material[] materials;
    [SerializeField] Image StarterImage;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in Buttons)
        {
            item.GetComponent<Renderer>().material = materials[0];
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) MouseFunctions(2);
        else if (Input.GetMouseButtonUp(0)) 
        {
            MouseFunctions(0); 
            if(MouseFunctions(0).collider.gameObject.name == "Start")
            {
                StarterImage.transform.Rotate(new Vector3(0, 0, 360) * 10 * Time.deltaTime);
                StarterImage.rectTransform.sizeDelta = new Vector2(1000, 1000) * 3 * Time.deltaTime;
                if(StarterImage.rectTransform.sizeDelta == new Vector2(1000, 1000)) { SceneManager.LoadScene(1); }
                //Place an animation in here to make the game more fun before loading the game
                
            }
        }
    }

    RaycastHit MouseFunctions(int i)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hit.collider.gameObject.GetComponent<Renderer>().material = materials[i];
        }
        return hit;
    }
}

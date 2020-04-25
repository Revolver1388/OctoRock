using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject[] Buttons;
    [SerializeField] Material[] materials;
    [SerializeField] Image StarterImage;

    bool startGame = false;
    bool options = false;

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
            if (MouseFunctions(0).collider.gameObject.name == "Start") startGame = true;
            else if (MouseFunctions(0).collider.gameObject.name == "Options") options = true;
            else if (MouseFunctions(0).collider.gameObject.name == "Quit") Application.Quit();
        }
        if (startGame)
        {
            StarterImage.transform.Rotate(new Vector3(0, 0, 360) * 1 * Time.deltaTime);
            StarterImage.rectTransform.sizeDelta = new Vector2(1000, 1000);
            if (StarterImage.rectTransform.sizeDelta == new Vector2(1000, 1000)) { StartCoroutine(Waitfer()); }
        }
    }

    IEnumerator Waitfer()
    {
        yield return new WaitForSeconds(3);
                SceneManager.LoadScene(1);            
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

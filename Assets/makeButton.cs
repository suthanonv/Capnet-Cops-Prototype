using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class makeButton : MonoBehaviour
{
    public UnityEvent unityEvent;
    public GameObject button;
    private Material defaultMaterial;
    public Material altMaterial;
    public Material clickedMaterial;
    private void Start()
    {
        defaultMaterial = GetComponent<Renderer>().material; //sets this as current material
        button = this.gameObject;
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
        {
            this.GetComponent<Renderer>().material = altMaterial;
            Debug.Log("Raycast Hit: Hovering");
        }
        else if(this.GetComponent<Renderer>().material != defaultMaterial)
        {
            this.GetComponent<Renderer>().material = defaultMaterial;
            Debug.Log("Raycast Gone: Reset to default.");
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject) 
            {
                this.GetComponent<Renderer>().material = clickedMaterial;
                Debug.Log("Raycast Hit && Clicked: Switched to click material");
                unityEvent.Invoke(); 
            }
        }
    }
}

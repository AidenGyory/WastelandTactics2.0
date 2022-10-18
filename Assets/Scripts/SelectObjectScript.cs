using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectObjectScript : MonoBehaviour
{
    //Raycast for Object Selection
    private Ray _ray;
    private RaycastHit _hit;

    public SelectScript highlightedObject;

    //check if mouse has moved 
    //bool for mouse move 
    //run raycast on mouse move 
    void Update()
    {
        RayCastToObjects();
    }

    void RayCastToObjects()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.transform.GetComponent<SelectScript>() != highlightedObject)
            {
                if (highlightedObject != null)
                {
                    highlightedObject.Unhighlight();
                }
                if (_hit.transform.GetComponent<SelectScript>() != null)
                {
                    _hit.transform.GetComponent<SelectScript>().Highlight();
                    highlightedObject = _hit.transform.GetComponent<SelectScript>();
                }
                else
                {
                    highlightedObject = null; 
                }
                 
            }
        }
    }
}

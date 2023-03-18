using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClickDrag : MonoBehaviour
{
    bool dragging;

    //Raycast for Object Selection
    private Ray _ray;
    private RaycastHit _hit;

    MoveScript _moveScript; 

    private void Start()
    {
        _moveScript = GetComponent<MoveScript>(); 
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            dragging = true;
        }
        if(Input.GetMouseButtonUp(1))
        {
            dragging=false;
        }


        if (dragging) 
        {
            // Ray equals the screen to point value of the screen to the mouse pointer 
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // CAST a ray out till it hits an object collider 
            if (Physics.Raycast(_ray, out _hit))
            {
                if (_hit.transform.GetComponent<SelectScript>() != null)
                {
                    _moveScript.SetDestination(_hit.transform.position);
                }
            }
        }
    }
}

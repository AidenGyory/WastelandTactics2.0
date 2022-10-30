using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectObjectScript : MonoBehaviour
{
    public static SelectObjectScript Instance;

    [SerializeField] HUDManager HUD;  
    //Raycast for Object Selection
    private Ray _ray;
    private RaycastHit _hit;

    public SelectScript highlightedObject;
    public SelectScript selectedObject;

    [SerializeField] Transform renderTextureModel; 

    //     -- TO OPTIMIZE --
    //     
    //  check if mouse has moved 
    //    bool for mouse move 
    // run raycast on mouse move 

    void Update()
    {
        RayCastToObjects();
        if(Input.GetMouseButtonDown(0))
        {
            if(selectedObject != null)
            {
                selectedObject.Deselect(); 
            }

            if(highlightedObject != null)
            {
                highlightedObject.Select();
                
                selectedObject = highlightedObject;
                HUD.DisplayObjectType(selectedObject);
                renderTextureModel.gameObject.SetActive(true);
                HUD.renderedTexture.SetActive(true);
                renderTextureModel.position = selectedObject.transform.position; 
            }
            else
            {
                selectedObject = null;
                renderTextureModel.gameObject.SetActive(false);
                HUD.renderedTexture.SetActive(false);
            }
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectObjectScript : MonoBehaviour
{
    //Raycast for Object Selection
    private Ray _ray;
    private RaycastHit _hit;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RayCastToObjects();
    }

    void RayCastToObjects()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SelectScript[] obj = FindObjectsOfType<SelectScript>();

                foreach (SelectScript _obj in obj)
                {
                    if (_obj.transform.GetComponent<SelectScript>().isHighlighted)
                    {
                        if (!_obj.transform.GetComponent<SelectScript>().isSelected)
                        {
                            _obj.isHighlighted = false;
                        }
                    }
                }
            }

            // Raycast to Highight Tiles, Buildings or Units 
            else if (_hit.transform.GetComponent<SelectScript>() != null)
            {
                _hit.transform.GetComponent<SelectScript>().Unhighlight(_hit.transform);

                _hit.transform.GetComponent<SelectScript>().Highlight();
            }

        }
    }
}

using UnityEngine;

public class SelectObjectScript : MonoBehaviour
{
    public enum PointerMode
    {
        SelectMode,
        MoveMode,
        PlacementMode,
    }

    public PointerMode mode; 
    [SerializeField] HUDManager HUD;  
    //Raycast for Object Selection
    private Ray _ray;
    private RaycastHit _hit;

    public SelectScript highlightedObject;
    public SelectScript selectedObject;

    void Update()
    {
        //Raycast down to collideable objects in scene 
        RayCastToObjects();

        //When mouse left click is pressed 
        if(Input.GetMouseButtonDown(0))
        {
            switch (mode)   
            {
                case PointerMode.SelectMode:
                    SelectModeInput();
                    break;
                case PointerMode.MoveMode:
                    MoveModeInput(); 
                    break;
                case PointerMode.PlacementMode:
                    PlaceModeInput(); 
                    break;
            }
        }
    }
    void SelectModeInput()
    {
        if (selectedObject != null && selectedObject != highlightedObject)
        {
            selectedObject.DeselectObject();
        }
        // if their is a highlighted object 
        if (highlightedObject != null)
        {
            //Set the Selected Object to the Highlighted Object 
            selectedObject = highlightedObject;
            // run the "Select" Function in the Select Script on Highlighted Object
            highlightedObject.SelectObject();

            // Display Object info from the Selected Object to the HUDManager Script
            HUD.DisplayObjectInfoHUD(selectedObject);

            if(selectedObject.type == SelectScript.objType.unit)
            {
                mode = PointerMode.MoveMode; 
            }
        }
        // If there is no Highlighted Object 
        else
        {
            // Set the selected Object to null to Deselect anything already stored 
            selectedObject = null;
            //Clear Selected Object from HUD
            HUD.ClearObjectInfoHUD();
        }
    }
    void MoveModeInput()
    {
        //Select a tile in MoveMode (Unit Mode) 
        if(highlightedObject.type == SelectScript.objType.tile)
        {
            TileInfo _tile = highlightedObject.GetComponent<TileInfo>();

            if(_tile.canWalk == TileInfo.IsWalkable.unset)
            {
                mode = PointerMode.SelectMode;
                selectedObject.DeselectObject();
                SelectModeInput(); 
                
            }
            else
            {
                if(highlightedObject.GetComponent<TileInfo>().canWalk == TileInfo.IsWalkable.canWalk)
                {
                    selectedObject.GetComponent<UnitInfo>().MoveUnit(highlightedObject.transform.position);
                }
                else
                {
                    Debug.Log("can't walk here"); 
                }
                
            }
        }
        if(highlightedObject.type == SelectScript.objType.structure)
        {
            mode = PointerMode.SelectMode;
            selectedObject.DeselectObject();
            SelectModeInput();
        }
    }
    void PlaceModeInput()
    {

    }
    void RayCastToObjects()
    {
        // Ray equals the screen to point value of the screen to the mouse pointer 
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // CAST a ray out till it hits an object collider 
        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.transform.GetComponent<SelectScript>() != null)
            {
                SelectScript _object = _hit.transform.GetComponent<SelectScript>();

                if(highlightedObject != null)
                {
                    highlightedObject.UnhighlightObject();
                }
                
                _object.HighlightObject();
                highlightedObject = _object;
                return; 
            }
            
            if (highlightedObject != null)
            {
                highlightedObject.UnhighlightObject();
                highlightedObject = null;
            }
        }
    }
}

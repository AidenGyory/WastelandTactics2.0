using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class SelectObjectScript : MonoBehaviour
{
    public static SelectObjectScript Instance; 
    public enum PointerMode
    {
        SelectMode,
        UnitMode,
        PlacementMode,
    }

    public PointerMode mode;   
    

    public SelectScript highlightedObject;
    public SelectScript selectedObject;

    public Transform CameraScreenCanvas;

    public bool canSelect;

    //Raycast for Object Selection
    private Ray _ray;
    private RaycastHit _hit;

    MoveScript _moveScript;
    CameraController _camScript;

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _moveScript = FindObjectOfType<MoveScript>();
        _camScript = FindObjectOfType<CameraController>(); 
    }
    void Update()
    {
        if(canSelect)
        {
            //Raycast down to Selectable objects in scene 
            RayCastToObjects();

            // Press "Left Click" 
            if (Input.GetMouseButtonDown(0))
            {
                if (highlightedObject != null)
                {
                    switch (mode)
                    {
                        case PointerMode.SelectMode:
                            if(highlightedObject.objectType == SelectScript.objType.structure && highlightedObject == selectedObject)
                            {
                                ActionModeInput();
                            }
                            else
                            SelectModeInput();

                            break;
                        case PointerMode.UnitMode:
                            UnitInput(); 

                            break;
                    }
                }
                else
                {
                    SetModeToSelect();
                    return; 
                }
            }
        }
        //REMEMBER TO ADD TOOLTIP HOVERING

    }
    void RayCastToObjects()
    {
        // Ray equals the screen to point value of the screen to the mouse pointer 
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // CAST a ray out till it hits an object collider 
        if (Physics.Raycast(_ray, out _hit))
        {
            //Guard for invalid raycast 
            if (_hit.transform.GetComponent<SelectScript>() == null && highlightedObject != null)
            {
                if (highlightedObject != null)
                {
                    highlightedObject.UnhighlightObject();
                    highlightedObject = null;
                }
                return;
            }
            else // _hit.transform.GetComponent<SelectScript>() == "something" 
            {
                //Guard for if raycast hits already highlighted object
                if (highlightedObject == _hit.transform.GetComponent<SelectScript>()) { return; }

                //Unhlight the current highlighted object
                if (highlightedObject != null)
                {
                    highlightedObject.UnhighlightObject();
                }

                // Highlighted object now equals the new raycast target 
                highlightedObject = _hit.transform.GetComponent<SelectScript>();
                highlightedObject.HighlightObject();
            }
        }

    }
    public void SetModeToSelect()
    {
        //Debug.Log("SetModeToSelect");
        ////Set pointer mode back to selectMode
        mode = PointerMode.SelectMode;
        //// Turn on Raycasting again
        canSelect = true;

        if (selectedObject != null)
        {
            ////Clear all Canvas info for Units 
            ClearActionIcons();
            ////Check which tiles can be flipped
            TileManager.instance.FindPlayerOwnedTilesForFlipCheck(GameManager.Instance.currentPlayerTurn);
            ////Deselect Everything
            DeselectAll();
        }

        Camera.main.GetComponent<CameraController>().SetCameraMode(CameraController.CameraMode.Unfocused);
    }
    void DeselectAll()
    {
        selectedObject.DeselectObject();
        selectedObject = null;

        _camScript.SetCameraMode(CameraController.CameraMode.Unfocused); 
    }
    void ClearActionIcons()
    {
        HealthScript[] _targets = FindObjectsOfType<HealthScript>();

        foreach (HealthScript _unit in _targets)
        {
            _unit.targetIcon.SetActive(false);
            _unit.isTarget = false; 
            
        }
    }
    public void SelectPlayer(PlayerInfo _player)
    {
        _moveScript.SetDestination(_player.transform.position);
        _camScript.SetCameraMode(CameraController.CameraMode.Focused); 
    }

    //INPUT MODES// 

    void SelectModeInput()
    {
        //Guard Selecting an already selected object 
        if (selectedObject == highlightedObject) { return; }

        //Is there already a Selected Object? 
        if (selectedObject != null)
        {
            selectedObject.DeselectObject();
        }

        selectedObject = highlightedObject;
        selectedObject.SelectObject();

        FocusObject(selectedObject.transform); 
    }
    void ActionModeInput()
    {
        switch (selectedObject.objectType)
        {
            case SelectScript.objType.tile:
                SetModeToSelect(); 

                break;
            case SelectScript.objType.structure:
                selectedObject.GetComponent<StructureInfo>().CheckAction();

                break;
            case SelectScript.objType.unit:
                selectedObject.GetComponent<UnitInfo>().CheckAction();

                break;
        }
    }

    void UnitInput()
    {
        if(highlightedObject.objectType == SelectScript.objType.tile)
        {
            MoveModeInput(); 
        }
        else
        {
            AttackUnitInput(); 
        }
    }
    void MoveModeInput()
    {
        //Debug.Log("Movement Input"); 
        if(highlightedObject.objectType == SelectScript.objType.tile && highlightedObject.GetComponent<TileInfo>().state == TileInfo.TileState.walkable)
        {
            // Execute "Move to tile" function 
            selectedObject.GetComponent<UnitInfo>().MoveToTile(highlightedObject.GetComponent<TileInfo>());

            // turn off raycasts 
            canSelect = false;
        }
        //Swapping between your own Units
        if(highlightedObject != selectedObject && highlightedObject.objectType == SelectScript.objType.unit)
        {
            if(highlightedObject.GetComponent<UnitInfo>().owner == selectedObject.GetComponent<UnitInfo>().owner)
            {
                SelectModeInput(); 
            }
        }
        else
        {
            SetModeToSelect();
        }
    }
    void AttackUnitInput()
    {
        //Guard for tile
        if(highlightedObject.objectType == SelectScript.objType.tile)
        {
            SetModeToSelect();
            return; 
        }

        //Guard against non-targets
        if(!highlightedObject.GetComponent<HealthScript>().isTarget)
        {
            SetModeToSelect();
            return;
        }

        HealthScript _target = highlightedObject.GetComponent<HealthScript>();

        _target.TakeDamage(selectedObject);
        selectedObject.GetComponent<UnitAttackScript>().hasAttacked = true; 
        SetModeToSelect();

    }
    public void FocusObject(Transform _target)
    {
        _moveScript.SetDestination(_target.position);
        _camScript.SetCameraMode(CameraController.CameraMode.Focused); 
    }
}

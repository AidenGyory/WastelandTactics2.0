using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class SelectObjectScript : MonoBehaviour
{
    public static SelectObjectScript Instance; 
    public enum PointerMode
    {
        SelectMode,
        MoveMode,
        PlacementMode,
    }

    public PointerMode mode;   
    //Raycast for Object Selection
    private Ray _ray;
    private RaycastHit _hit;

    public SelectScript highlightedObject;
    public SelectScript selectedObject;

    public Transform CameraScreenCanvas;

    public bool canSelect;

    public MoveScript moveScript;
    public CameraController camScript;

    public MoveLineScript _line; 
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        moveScript = FindObjectOfType<MoveScript>();
        camScript = FindObjectOfType<CameraController>(); 
    }
    void Update()
    {
        if(canSelect)
        {
            //Raycast down to collideable objects in scene 
            RayCastToObjects();

            //When mouse left click is pressed 
            if (Input.GetMouseButtonDown(0))
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
                        PlacementModeInput(); 
                        //PlaceModeInput(); 
                        break;
                }
            }
            //Right-click for flagging locations
            if (Input.GetMouseButtonDown(1))
            {
                if (highlightedObject.objectType == SelectScript.objType.tile)
                {
                    highlightedObject.GetComponent<TileInfo>().ToggleFlagState();
                }
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(selectedObject != null)
                {
                    moveScript.SetDestination(selectedObject.transform.position);
                    camScript.SetCameraMode(CameraController.CameraMode.Focused); 
                }
            }
            
        }



        //Add a tooltip function to tile hovering 

        
    }
    void RayCastToObjects()
    {
        // Ray equals the screen to point value of the screen to the mouse pointer 
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // CAST a ray out till it hits an object collider 
        if (Physics.Raycast(_ray, out _hit))
        {

            if (_hit.transform.GetComponent<SelectScript>() == null && highlightedObject != null)
            {
                if(highlightedObject != null)
                {
                    highlightedObject.UnhighlightObject();
                    highlightedObject = null;
                }
                return; 
            }
            else // _hit.transform.GetComponent<SelectScript>() == "something" 
            {
                if(highlightedObject == _hit.transform.GetComponent<SelectScript>()) { return; }

                else if(highlightedObject != null)
                {
                    highlightedObject.UnhighlightObject();  
                }

                highlightedObject = _hit.transform.GetComponent<SelectScript>();
                highlightedObject.HighlightObject();
                return; 
            }
        }
        
    }
    void SelectModeInput()
    {
        //TIleAudioManager.instance.PlayTileAudio(tileAudioType.select); 

        //once left click is pressed check if there is already an object selected. 
        if(selectedObject != null)
        {
            //if so, if the object you are selecting is the same object then focus camera on it. 
            if (selectedObject == highlightedObject)
            {
                moveScript.SetDestination(selectedObject.transform.position);
                camScript.SetCameraMode(CameraController.CameraMode.Focused);
            }
            else
            {
                selectedObject.DeselectObject();
                selectedObject = null;
                camScript.SetCameraMode(CameraController.CameraMode.Unfocused);
            }
            
        }

        if (highlightedObject != null)
        {
            selectedObject = highlightedObject;
            selectedObject.SelectObject();

            
        }

        //Guard for null clause
        if(selectedObject == null) { return; }

        if (selectedObject.objectType == SelectScript.objType.tile)
        {
            selectedObject.GetComponent<TileInfo>().TryToFlipTile();
        }
    }

    public void PlacementModeInput()
    {
        
    }

    public void MoveModeInput()
    {

        //Check that the highlighted object is a tile 
        if(highlightedObject != null && highlightedObject.objectType == SelectScript.objType.tile)
        {
            // Get reference to highlighted tile 
            TileInfo _tile = highlightedObject.GetComponent<TileInfo>();

            //If tile is walkable 
            if (_tile.state == TileInfo.TileState.walkable)
            {
                //get reference to Unit
                UnitInfo _unit = selectedObject.GetComponent<UnitInfo>();

                // Execute "Move to tile" function 
                _unit.MoveToTile(_tile);

                // turn off selectable 
                canSelect = false;
            }
            else if (_tile.state == TileInfo.TileState.IsFlipped || _tile.state == TileInfo.TileState.CannotFlip)
            {
                SetModeToSelect();
            }




            if (selectedObject != null)
            {
                

            }
        }
        else
        {
            SetModeToSelect();
        }
    }

    public void SetModeToSelect()
    {
        canSelect = true;
        mode = PointerMode.SelectMode;
        camScript.mode = CameraController.CameraMode.Unfocused;  
        selectedObject.DeselectObject(); 

        if (GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn].ExplorationPointsLeft > 0)
        {
            TileManager.instance.FindPlayerOwnedTilesForFlipCheck(GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn]); 
        }

    }
}

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
                        //MoveModeInput(); 
                        break;
                    case PointerMode.PlacementMode:
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
}

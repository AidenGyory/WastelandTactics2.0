using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuForUnits : MonoBehaviour
{
    // Rference to Radial Menu
    [SerializeField] GameObject RadialMenu;
    
    //reference to Move input
    Vector2 moveInput; 
    
    // References to external options
    [SerializeField] GameObject[] options;
    [SerializeField] GameObject exitButton; 
    [SerializeField] int optionSelected; 
    // 360 / number of options 
    [SerializeField] float optionsThreshold; 

    //colours for text
    [SerializeField] Color standard, highlight, notEnoughResources;
    [Header("Center Text")]
    [SerializeField] TMP_Text unitSelectedText;
    [SerializeField] TMP_Text resourceCostText;
    [SerializeField] TMP_Text resourceCostNumber;
    [SerializeField] Image selectedIcon;

    [SerializeField] GameObject UnitPlaceablePrefab; 
    
    public void OpenRadialMenu()
    {
        
        Camera.main.GetComponent<CameraController>().SetCameraMode(CameraController.CameraMode.Zoomed);
        RadialMenu.SetActive(true); 
    }

    public void CloseRadialMenu()
    {
        RadialMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(RadialMenu.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseRadialMenu(); 
            }

            moveInput.x = Input.mousePosition.x - (Screen.width / 2f);
            moveInput.y = Input.mousePosition.y - (Screen.height / 2f);

            float _distance = Vector2.Distance(Vector2.zero, moveInput);
            //Debug.Log(_distance);

            moveInput.Normalize();

            

            if (moveInput != Vector2.zero)
            {
                
                
                float _angle = Mathf.Atan2(moveInput.y, -moveInput.x) / Mathf.PI;
                _angle *= 180f;
                _angle -= 90f; 
                if(_angle < 0)
                {
                    _angle += 360; 
                }

                if(_distance <90 || _distance > 220)
                {
                    unitSelectedText.text = "";
                    resourceCostText.text = "";
                    resourceCostNumber.text = "";

                    optionSelected = 0;
                    selectedIcon.color = Color.clear;
                }


                //Debug.Log(_angle);

                for (int i = 0; i < options.Length; i++)
                {
                    //segments are 360 / number of options 
                    if (_angle > i * optionsThreshold && _angle < (i +1) * optionsThreshold && _distance > 90 && _distance < 220)
                    {
                        //select option
                        optionSelected = i;
                        options[i].GetComponent<RadialOptionScript>().HighlightSegment(highlight);
                        unitSelectedText.text = "" + options[i].GetComponent<RadialOptionScript>().unitName;
                        resourceCostNumber.text = "" + options[i].GetComponent<RadialOptionScript>().resourceCost;
                        resourceCostText.text = "Cost:";

                        //Cancel Option
                        if(optionSelected == 2)
                        {
                            resourceCostNumber.text = "";
                            resourceCostText.text = "";
                        }

                        //Set color for Cost Text
                        if (GameManager.Instance.currentPlayerTurn.MetalScrapAmount < options[optionSelected].GetComponent<RadialOptionScript>().resourceCost)
                        {
                            resourceCostNumber.color = notEnoughResources; 
                        }
                        else
                        {
                            resourceCostNumber.color = Color.white; 
                        }

                        selectedIcon.sprite = options[i].GetComponent<RadialOptionScript>().icon.sprite;
                        selectedIcon.color = new Color(1, 1, 1, 0.7f); 


                    }
                    else
                    {
                        options[i].GetComponent<RadialOptionScript>().UnHighlightSegment(standard);
                    }
                }

                
            }
            if(Input.GetMouseButtonDown(0))
            {
                if(_distance < 90 || _distance > 220 )
                {

                    SelectObjectScript.Instance.SetModeToSelect(); 
                    CloseRadialMenu();
                }
                else
                {
                    if (GameManager.Instance.currentPlayerTurn.MetalScrapAmount >= options[optionSelected].GetComponent<RadialOptionScript>().resourceCost)
                    {
                        if(optionSelected == 2)
                        {
                            SelectObjectScript.Instance.SetModeToSelect();
                            CloseRadialMenu();
                        }
                        else
                        {
                            PlaceUnit();
                        }
                    }
                    else
                    {
                        TileAudioManager.instance.PlayTileAudio(tileAudioType.negflip); 
                        Debug.Log("Not Enough Metal!!!"); 
                    }
                }
            }
        }
    }

    public void PlaceUnit()
    {
        Camera.main.GetComponent<CameraController>().SetCameraMode(CameraController.CameraMode.Topdown);

        GameObject _placeable = Instantiate(UnitPlaceablePrefab);
        _placeable.GetComponent<PlaceUnitController>().cost = options[optionSelected].GetComponent<RadialOptionScript>().resourceCost; 


        switch (optionSelected)
        {
            case 0: //Scout

                _placeable.GetComponent<PlaceUnitController>().type = PlaceUnitController.UnitTypes.Scout_NewEden;

                break;
            case 1: //Tank
                _placeable.GetComponent<PlaceUnitController>().type = PlaceUnitController.UnitTypes.Tank_NewEden;

                break;
            case 2: //CANCEL
                CloseRadialMenu(); 

                break;
            case 3: //AntiGun 
                _placeable.GetComponent<PlaceUnitController>().type = PlaceUnitController.UnitTypes.AntiTank_NewEden;

                break;
            case 4: //Soldier
                _placeable.GetComponent<PlaceUnitController>().type = PlaceUnitController.UnitTypes.Soldier_NewEden;

                break;
        }

        SelectObjectScript.Instance.canSelect = true;
        SelectObjectScript.Instance.mode = SelectObjectScript.PointerMode.PlacementMode;
        TileManager.instance.ClearCanFlipStateOnAllTiles();



        //Grab all tiles in border 
        TileInfo _OccupiedTile = SelectObjectScript.Instance.selectedObject.GetComponent<StructureInfo>().occupiedTile;

        List<TileInfo> _placementTiles = TileManager.instance.SetTileList(_OccupiedTile.transform.position, 1); 

        foreach (TileInfo _tile in _placementTiles)
        {
            Debug.Log("Tile: " + _tile.name); 
            _tile.SetTileToPlaceable();
        }

        CloseRadialMenu();
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuForStructures : MonoBehaviour
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

    [SerializeField] GameObject BuildingPlaceablePrefab; 
    
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
                            PlaceBuilding();
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

    public void PlaceBuilding()
    {
        Camera.main.GetComponent<CameraController>().SetCameraMode(CameraController.CameraMode.Topdown);

        GameObject _placeable = Instantiate(BuildingPlaceablePrefab);
        _placeable.GetComponent<PlaceStructureScript>().cost = options[optionSelected].GetComponent<RadialOptionScript>().resourceCost; 


        switch (optionSelected)
        {
            case 0: //Outpost

                _placeable.GetComponent<PlaceStructureScript>().type = PlaceStructureScript.BuildingTypes.Outpost_NewEden; 

                break;
            case 1: //Factory
                _placeable.GetComponent<PlaceStructureScript>().type = PlaceStructureScript.BuildingTypes.Factory_NewEden; 

                break;
            case 2: //CANCEL
                CloseRadialMenu(); 

                break;
            case 3: //Power Generator
                _placeable.GetComponent<PlaceStructureScript>().type = PlaceStructureScript.BuildingTypes.Power_NewEden; 

                break;
            case 4: //Research Centre
                _placeable.GetComponent<PlaceStructureScript>().type = PlaceStructureScript.BuildingTypes.Research_NewEden; 

                break;
        }

        SelectObjectScript.Instance.canSelect = true;
        SelectObjectScript.Instance.mode = SelectObjectScript.PointerMode.PlacementMode;
        TileManager.instance.ClearCanFlipStateOnAllTiles();

        TileManager.instance.SetBorderTileOwnership(); 

        //Grab all tiles in border 
        List<TileInfo> _placementTiles = new List<TileInfo>();

        foreach (TileInfo _tile in TileManager.instance.allTiles)
        {
            if(_tile.Owner == GameManager.Instance.currentPlayerTurn)
            {
                if(_tile.BorderOwner == GameManager.Instance.currentPlayerTurn)
                {
                    _placementTiles.Add(_tile);
                }
            }
        }

        foreach (TileInfo _tile in _placementTiles)
        {
            switch (optionSelected)
            {
                case 0: //Outpost

                    if (_tile.type == TileInfo.TileType.MetalMine || _tile.type == TileInfo.TileType.Unhexium || _tile.type == TileInfo.TileType.Mountain)
                    {
                        
                    }
                    else
                    {
                        _tile.SetTileToPlaceable();
                    }

                    break;
                case 1: //Factory
                    if(_tile.type == TileInfo.TileType.MetalMine || _tile.type == TileInfo.TileType.Unhexium)
                    {
                        _tile.SetTileToPlaceable();
                    }

                    break;
                case 3: //Power Generator
                    _tile.SetTileToPlaceable();

                    break;
                case 4: //Research Centre
                    if (_tile.type == TileInfo.TileType.MetalMine || _tile.type == TileInfo.TileType.Unhexium)
                    {

                    }
                    else
                    {
                        _tile.SetTileToPlaceable();
                    }

                    break;
            }
        }

        CloseRadialMenu();
    }
}

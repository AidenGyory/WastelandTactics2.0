using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuController : MonoBehaviour
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

    [SerializeField] GameObject unitPlaceablePrefab; 
    
    public void OpenRadialMenu()
    {
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

                        //Set color for Cost Text
                        if (GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn].MetalScrapAmount < options[optionSelected].GetComponent<RadialOptionScript>().resourceCost)
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
                    
                    SelectObjectScript.Instance.canSelect = true;
                    CloseRadialMenu();
                }
                else
                {
                    if (GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn].MetalScrapAmount >= options[optionSelected].GetComponent<RadialOptionScript>().resourceCost)
                    {
                        PlaceUnit(); 
                        //Invoke(nameof(PlaceUnit), 0.2f);  
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

        GameObject _placeable = Instantiate(unitPlaceablePrefab);
        _placeable.GetComponent<PlaceUnitController>().cost = options[optionSelected].GetComponent<RadialOptionScript>().resourceCost; 


        switch (optionSelected)
        {
            case 0: //Scout Unit
                //Debug.Log("Spawn Scout Unit");
                _placeable.GetComponent<PlaceUnitController>().type = PlaceUnitController.UnitTypes.Scout_NewEden;


                break;
            case 1: //Soldier Unit 
                Debug.Log("Spawn Soldier Unit");
                _placeable.GetComponent<PlaceUnitController>().type = PlaceUnitController.UnitTypes.Soldier_NewEden;



                break;
            case 2: //Tank Unit
                Debug.Log("Spawn Tank Unit");
                _placeable.GetComponent<PlaceUnitController>().type = PlaceUnitController.UnitTypes.Tank_NewEden;



                break;
            case 3: //Anti-Tank Unit
                Debug.Log("Spawn Anti-Tank Unit");
                _placeable.GetComponent<PlaceUnitController>().type = PlaceUnitController.UnitTypes.AntiTank_NewEden;



                break;
            case 4: //Worker Unit 
                Debug.Log("Spawn Worker Unit");
                _placeable.GetComponent<PlaceUnitController>().type = PlaceUnitController.UnitTypes.Worker_NewEden;



                break;
        }
        SelectObjectScript.Instance.canSelect = true;
        SelectObjectScript.Instance.mode = SelectObjectScript.PointerMode.PlacementMode;
        TileManager.instance.ClearCanFlipStateOnAllTiles();
        List<TileInfo> _placementTiles = TileManager.instance.SetTileList(GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn].transform.position, 1);

        foreach (TileInfo _tile in _placementTiles)
        {
            _tile.SetTileToPlaceable();
        }

        CloseRadialMenu();
    }
}

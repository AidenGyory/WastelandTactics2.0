using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayHUDInfoScript : MonoBehaviour
{
    //HUD COMPONENTS 

    [Header("Map Filters")]
    [SerializeField] bool FiltersMaximised;
    [SerializeField] GameObject MapFilterObject;

    [Header("Pass Turn Info")]
    [SerializeField] GameObject EPIcon; 
    [SerializeField] TMP_Text EPLeftText;
    [SerializeField] GameObject PassTurnIcon;

    [Header("Selected Object Info")]
    [SerializeField] GameObject ObjInfoHUD; 
    [SerializeField] TMP_Text ObjNameText;
    [SerializeField] Image ObjIcon;
    [SerializeField] GameObject healthBarUI; 
    [SerializeField] Image healthbarFill;
    [SerializeField] TMP_Text healthbarText;
    [SerializeField] GameObject ActionPointUI; 
    [SerializeField] TMP_Text ActionPoints;
    [SerializeField] GameObject[] levels;
    [Space]
    [SerializeField] GameObject UnitStatUI; 
    [SerializeField] TMP_Text UnitAttack; 
    [SerializeField] TMP_Text UnitRange;
    [Space]
    [SerializeField] GameObject StructureStatUI;
    [SerializeField] TMP_Text StructureSight;
    [Space]
    [SerializeField] GameObject TilesUI;
    [SerializeField] TMP_Text tileDescription;

    [Header("Player Resources")]
    [SerializeField] TMP_Text UnhexiumAmount;
    [SerializeField] TMP_Text MetalScrapAmount;
    [SerializeField] TMP_Text TurnTimer;
    [SerializeField] GameObject PowerPointer;
    [SerializeField] GameObject PowerBar;
    [SerializeField] Image PlayerFlag;
    [SerializeField] Image PlayerIcon; 

    [Header("Game Menu Buttons")]
    [SerializeField] UnityEvent GlossaryButton;
    [SerializeField] UnityEvent GameMenuButton;

    PlayerInfo _player;
    SelectScript _selected; 


    public void UpdateHUDInfo()
    {
        _player = GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn];
        _selected = SelectObjectScript.Instance.selectedObject; 

        if(_selected != null)
        {
            ObjInfoHUD.SetActive(true); 
            UpdateSelectedInfo();
        }
        else
        {
            ObjInfoHUD.SetActive(false); 
        }

        UpdateResources(); 
    }

    void UpdateSelectedInfo()
    {
        foreach (GameObject _level in levels)
        {
            _level.SetActive(false);
        }

        switch (_selected.objectType)
        {
            case SelectScript.objType.tile:
            {
                    StructureStatUI.SetActive(false);
                    UnitStatUI.SetActive(false);
                    healthBarUI.SetActive(false);

                    TilesUI.SetActive(true);
                    ActionPointUI.SetActive(true);

                    

                    //TileInfo
                    TileInfo _info = _selected.GetComponent<TileInfo>();
                    if(_info.state != TileInfo.TileState.IsFlipped)
                    {
                        ObjNameText.text = "Unknown"; 

                    }
                    else
                    {
                        ObjNameText.text = "" + _info.tileName;
                        ObjIcon.sprite = _info.tileImage;
                    }
                    

                }
                break;
            case SelectScript.objType.structure:
            {
                    TilesUI.SetActive(false);
                    ActionPointUI.SetActive(false);
                    UnitStatUI.SetActive(false);

                    StructureStatUI.SetActive(true);
                    healthBarUI.SetActive(true);



                    //StructureInfo
                    StructureInfo _info = _selected.GetComponent<StructureInfo>();

                    ObjNameText.text = "" + _info.StructureName;
                    ObjIcon.sprite = _info.StructureImage;
                    healthbarText.text = "" + _info.currentHealth + "/" + _info.maxHealth;
                    healthbarFill.fillAmount = _info.currentHealth / _info.maxHealth;

                    

                    for (int i = 0; i < _info.UpgradeLevel; i++)
                    {
                        levels[i].SetActive(true);
                    }

                    StructureSight.text = "" + _info.sightRangeInTiles;
                }
                break;
            case SelectScript.objType.unit:
            {
                    UnitStatUI.SetActive(true);
                    ActionPointUI.SetActive(true);
                    healthBarUI.SetActive(true);

                    TilesUI.SetActive(false);
                    StructureStatUI.SetActive(false);
                    

                    //UnitInfo
                    UnitInfo _info = _selected.GetComponent<UnitInfo>();

                    ObjNameText.text = "" + _info.unitName;
                    ObjIcon.sprite = _info.unitImage; 
                    healthbarText.text = "" + _info.currentHealth + "/" + _info.maxHealth;
                    healthbarFill.fillAmount = _info.currentHealth/_info.maxHealth;
                    ActionPoints.text = "" + _info.currentMovementTiles;

                    foreach(GameObject _level in levels)
                    {
                        _level.SetActive(false); 
                    }

                    for (int i = 0; i < _info.prestigeLevel; i++)
                    {
                        levels[i].SetActive(true); 
                    }

                    UnitAttack.text = "" + _info.baseDamage;
                    UnitRange.text = "" + _info.attackRange; 

                }
                break; 
            
        }
    }

    void UpdateResources()
    {
        if(_player.ExplorationPointsLeft > 0)
        {
            EPLeftText.text = "" + _player.ExplorationPointsLeft;
            EPIcon.SetActive(true);
            PassTurnIcon.SetActive(false);

        }
        else
        {
            EPIcon.SetActive(false);
            EPLeftText.text = " ";
            PassTurnIcon.SetActive(true); 
        }
        UnhexiumAmount.text = "" + _player.UnhexiumNodesCaptured; 
        MetalScrapAmount.text = "" + _player.MetalScrapAmount;
        TurnTimer.text = "Turn: " + GameManager.Instance.turnTimer;
        
        //MAKE SURE TO REMEMBER TO ADD POWER CELL MAINTENECE 
    }

    public void PassTurn()
    {
        GameManager.Instance.SetNextPlayersTurn(); 
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUDInfo(); 
    }


}

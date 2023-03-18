using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayHUDInfoScript : MonoBehaviour
{

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
    [SerializeField] GameObject PowerBar;
    [SerializeField] GameObject[] UnhexiumAmount;
    [SerializeField] GameObject MalfunctionText; 
    [SerializeField] TMP_Text MetalScrapAmount;
    [SerializeField] TMP_Text ResearchPoints;
    [SerializeField] TMP_Text TurnTimer;
    [SerializeField] TMP_Text playerName; 
    [SerializeField] Image PlayerFlag;
    [SerializeField] Image PlayerIcon; 

    [Header("Game Menu Buttons")]
    [SerializeField] UnityEvent GlossaryButton;
    [SerializeField] UnityEvent GameMenuButton;

    [Header("Additional UI Elements")]
    [SerializeField] Image nameplate;
    [SerializeField] Image metalIcon; 
    [SerializeField] Image metalIconBG;
    [SerializeField] Image metalIconTextBG;
    [SerializeField] Image researchIcon; 
    [SerializeField] Image researchIconBG;
    [SerializeField] Image researchTextBG;
    [SerializeField] Image powerIconBG;
    [SerializeField] Image powerBarBG;
    [SerializeField] Image powerBarIconColour; 

    [Header("InfluenceBar")]
    [SerializeField] Image player1TileInfluence; 
    [SerializeField] Image player2TileInfluence;

    PlayerInfo _player;
    SelectScript _selected; 

    public void UpdateHUDInfo()
    {
        _player = GameManager.Instance.currentPlayerTurn;
        _selected = SelectObjectScript.Instance.selectedObject; 

        if(_selected != null)
        {
            ObjInfoHUD.SetActive(true); 
            //UpdateSelectedInfo();
        }
        else
        {
            ObjInfoHUD.SetActive(false); 
        }

        UpdateResources();

        UpdateUI();

    }

    void UpdateUI()
    {
        PlayerIcon.sprite = _player.settings.factionLogo; 

        Color primary = _player.settings.primaryColour;
        Color secondary = _player.settings.secondaryColour;

        PlayerFlag.color = primary;
        PlayerIcon.color = secondary;

        nameplate.color = secondary;

        metalIcon.color = secondary;
        metalIconBG.color = primary;
        metalIconTextBG.color = secondary;

        researchIcon.color = secondary;
        researchIconBG.color = primary;
        researchTextBG.color = secondary;

        powerIconBG.color = secondary;
        powerBarBG.color = secondary;
    }

    //void UpdateSelectedInfo()
    //{
    //    foreach (GameObject _level in levels)
    //    {
    //        _level.SetActive(false);
    //    }

    //    switch (_selected.objectType)
    //    {
    //        case SelectScript.objType.tile:
    //        {
    //                StructureStatUI.SetActive(false);
    //                UnitStatUI.SetActive(false);
    //                healthBarUI.SetActive(false);

    //                TilesUI.SetActive(true);
    //                ActionPointUI.SetActive(true);

                    

    //                //TileInfo
    //                TileInfo _info = _selected.GetComponent<TileInfo>();
    //                if(_info.state != TileInfo.TileState.IsFlipped)
    //                {
    //                    ObjNameText.text = "Unknown"; 

    //                }
    //                else
    //                {
    //                    ObjNameText.text = "" + _info.tileName;
    //                    ObjIcon.sprite = _info.tileImage;
    //                }
                    

    //            }
    //            break;
    //        case SelectScript.objType.structure:
    //        {
    //                TilesUI.SetActive(false);
    //                ActionPointUI.SetActive(false);
    //                UnitStatUI.SetActive(false);

    //                StructureStatUI.SetActive(true);
    //                healthBarUI.SetActive(true);



    //                //StructureInfo
    //                StructureInfo _info = _selected.GetComponent<StructureInfo>();

    //                ObjNameText.text = "" + _info.StructureName;
    //                ObjIcon.sprite = _info.StructureImage;
    //                healthbarText.text = "" + _info.currentHealth + "/" + _info.maxHealth;
    //                healthbarFill.fillAmount = _info.currentHealth / _info.maxHealth;

                    

    //                for (int i = 0; i < _info.UpgradeLevel; i++)
    //                {
    //                    levels[i].SetActive(true);
    //                }

    //                StructureSight.text = "" + _info.sightRangeInTiles;
    //            }
    //            break;
    //        case SelectScript.objType.unit:
    //        {
    //                UnitStatUI.SetActive(true);
    //                ActionPointUI.SetActive(true);
    //                healthBarUI.SetActive(true);

    //                TilesUI.SetActive(false);
    //                StructureStatUI.SetActive(false);
                    

    //                //UnitInfo
    //                UnitInfo _info = _selected.GetComponent<UnitInfo>();

    //                ObjNameText.text = "" + _info.unitName;
    //                ObjIcon.sprite = _info.unitImage; 
    //                healthbarText.text = "" + _info.currentHealth + "/" + _info.maxHealth;
    //                healthbarFill.fillAmount = _info.currentHealth/_info.maxHealth;
    //                ActionPoints.text = "" + _info.currentMovementTiles;

    //                foreach(GameObject _level in levels)
    //                {
    //                    _level.SetActive(false); 
    //                }

    //                for (int i = 0; i < _info.prestigeLevel; i++)
    //                {
    //                    levels[i].SetActive(true); 
    //                }

    //                UnitAttack.text = "" + _info.baseDamage;
    //                UnitRange.text = "" + _info.attackRange; 

    //            }
    //            break; 
            
    //    }
    //}

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

        

        MetalScrapAmount.text = "" + _player.MetalScrapAmount;
        ResearchPoints.text = "" + _player.ResearchPoints; 
        playerName.text = "" + _player.settings.playerName;
        int turnsleft = 30 - GameManager.Instance.turnTimer; 
        TurnTimer.text = "SANDSTORM IN: " + turnsleft;


        //Reset PowerCells
        foreach (GameObject _cell in UnhexiumAmount)
        {
            _cell.SetActive(false); 
        }

        int _cellsToDisplay = _player.PowerSupplyTotal + 1; 

        if(_cellsToDisplay > UnhexiumAmount.Length)
        {
            _cellsToDisplay = UnhexiumAmount.Length; 
        }

        for (int i = 0; i < _cellsToDisplay; i++)
        {
            UnhexiumAmount[i].SetActive(true); 
        }


        for (int i = 0; i < UnhexiumAmount.Length; i++)
        {
            UnhexiumAmount[i].GetComponent<CellInfo>().ChangeColor(CellInfo.PowerState.none);
        }

        //if in Deficit
        if(_player.PowerSupplyUsed > _player.PowerSupplyTotal)
        {
            for (int i = 0; i < _player.PowerSupplyUsed; i++)
            {
                UnhexiumAmount[i].GetComponent<CellInfo>().ChangeColor(CellInfo.PowerState.notCollected);
            }
            for (int i = 0; i < _player.PowerSupplyTotal; i++)
            {
                UnhexiumAmount[i].GetComponent<CellInfo>().ChangeColor(CellInfo.PowerState.inDeficit);
            }
            MalfunctionText.SetActive(true);
            powerBarIconColour.color = Color.red; 
        }
        else
        {
            //Power Cells Collected
            for (int i = 0; i < _player.PowerSupplyTotal; i++)
            {
                UnhexiumAmount[i].GetComponent<CellInfo>().ChangeColor(CellInfo.PowerState.collected);
            }

            //Power Cells Used 
            for (int i = 0; i < _player.PowerSupplyUsed; i++)
            {
                UnhexiumAmount[i].GetComponent<CellInfo>().ChangeColor(CellInfo.PowerState.used);
            }
            MalfunctionText.SetActive(false);
            powerBarIconColour.color = Color.cyan;
        }

    }

    public void PassTurn()
    {
        GameManager.Instance.SetNextPlayersTurn(); 
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUDInfo();
        CheckInfluence(); 
    }

    public void CheckInfluence()
    {
        float ownedTiles = 0;
        float player1Tiles = 0;
        float player2Tiles = 0;

        for (int i = 0; i < TileManager.instance.allTiles.Count; i++)
        {
            if (TileManager.instance.allTiles[i].Owner != null)
            {
                ownedTiles+=1;
                if (TileManager.instance.allTiles[i].Owner == GameManager.Instance.players[0])
                {
                    player1Tiles += 1;
                }
                if (TileManager.instance.allTiles[i].Owner == GameManager.Instance.players[1])
                {
                    player2Tiles += 1;
                }
            }
        }

        player1TileInfluence.fillAmount = player1Tiles / ownedTiles;
        player2TileInfluence.fillAmount = player2Tiles / ownedTiles;

    }

}

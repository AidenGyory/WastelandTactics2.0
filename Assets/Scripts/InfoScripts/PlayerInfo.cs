using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TechTreeOption
{
    [Header("Tech Upgrade Components")]
    public string upgradeName;
    [TextArea]
    public string description;
    public int pointCost;
    public bool alreadyResearched;

}
public partial class PlayerInfo : MonoBehaviour
{
    public TechTreeOption[] agressiveUpgradeTrack;
    public TechTreeOption[] defensiveUpgradeTrack;
    public TechTreeOption[] explotiveUpgradeTrack;

    public PlayerSettingsTemplate settings;

    [Header("Resources")]
    public int MetalScrapAmount; //The Amount of MetalScrap Resources you have in Total 
    public int ExplorationPointsMax; //The maximum amount of points you have to flip tiles 
    public int ExplorationPointsLeft; //How many points you haveleft this turn. 
    public int PowerSupplyTotal; //The Amount of Power output by generators and HQ
    public int PowerSupplyUsed; //Amount of Power Used 
    public int ResearchPoints; // Points used for researching things 
    public int SuperPowerPoints; //Used to unleash Super Powers
    [Space]
    public int InfluencePoints; 
    public int VictoryPoints; //Score at end of game to see who wins

    [Header("Research Conducted")]
    public bool ExtraSuperPoints;
    public bool TankUnlock;
    public bool IncreasedDamage;
    public bool PrestigeUpgrade;
    public bool CreateSuperSoldier;
    [Space]
    public bool RelicFind;
    public bool ScannerRange;
    public bool BorderUpgrade;
    public bool DecreasedMalfunctions;
    public bool CreateSuperBuilding;
    [Space]
    public bool MetalProduction;
    public bool GeneratorUnlock;
    public bool ExtraBattery;
    public bool ReducedCosts;
    public bool RemoveMalfunction; 

    public void AddExplorationPoints(int _amount)
    {
        ExplorationPointsLeft += _amount;

        if (ExplorationPointsLeft < 0)
        {
            ExplorationPointsLeft = 0;
        }
        if (ExplorationPointsLeft > ExplorationPointsMax)
        {
            ExplorationPointsLeft = ExplorationPointsMax;
        }
    }

    public void AddSuperPowerPoints()
    {
        SuperPowerPoints++; 
        if(ExtraSuperPoints)
        {
            SuperPowerPoints++; 
        }
    }

    public void AddMetalScrap(int _amount)
    {
        Debug.Log("give " + _amount + " Scrap Metal"); 
        MetalScrapAmount += _amount; 
    }

    public void UpdatePlayerPowerSupply()
    {
        UnitInfo[] _units = FindObjectsOfType<UnitInfo>();
        StructureInfo[] _Structures = FindObjectsOfType<StructureInfo>();

        int _powerUsed = 0;

        for (int i = 0; i < _units.Length; i++)
        {
            if (_units[i].owner == this)
            {
                _powerUsed += _units[i].powerCost;
            }
        }

        for (int i = 0; i < _Structures.Length; i++)
        {
            if (_Structures[i].owner == this)
            {
                _powerUsed += _Structures[i].powerCost;
            }
        }

        PowerSupplyUsed = _powerUsed;

    }

    public void CheckForMalfunctions()
    {
        UpdatePlayerPowerSupply();

        if(PowerSupplyUsed > PowerSupplyTotal)
        {
            Debug.Log(settings.playerName + " is in a power defict!!");
        }

        
    }
    public void PurchaseTrack1Upgrade(int _index)
    {
        switch (_index)
        {
            case 0:
                ExtraSuperPoints = true;
                break;
            case 1:
                TankUnlock = true;
                break;
            case 2:
                IncreasedDamage = true;
                break;
            case 3:
                PrestigeUpgrade = true;
                break;
            case 4:
                CreateSuperSoldier = true;
                break;
        }

        agressiveUpgradeTrack[_index].alreadyResearched = true;
    }

    public void PurchaseTrack2Upgrade(int _index)
    {

        switch (_index)
        {
            case 0:
                RelicFind = true;
                break;
            case 1:
                ScannerRange = true;
                break;
            case 2:
                BorderUpgrade = true;
                break;
            case 3:
                DecreasedMalfunctions = true;
                break;
            case 4:
                CreateSuperBuilding = true;
                break;
        }

        defensiveUpgradeTrack[_index].alreadyResearched = true;
    }

    public void PurchaseTrack3Upgrade(int _index)
    {

        switch (_index)
        {
            case 0:
                MetalProduction = true;
                break;
            case 1:
                GeneratorUnlock = true;
                break;
            case 2:
                ExtraBattery = true;
                break;
            case 3:
                ReducedCosts = true;
                break;
            case 4:
                RemoveMalfunction = true;
                break;
        }

        explotiveUpgradeTrack[_index].alreadyResearched = true;
    }
}

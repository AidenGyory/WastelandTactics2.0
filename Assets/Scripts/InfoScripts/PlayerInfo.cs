using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Test
{
    Hi,
    Aiden
}

public partial class PlayerInfo : MonoBehaviour
{

    public PlayerSettingsTemplate settings;

    [Header("Resources")]
    public int VictoryPoints; //Score at end of game to see who wins
    public int UnhexiumNodesCaptured; //The amount of Unhexium points captured on the map
    public int MetalScrapAmount; //The Amount of MetalScrap Resources you have in Total
    public int MetalScrapProduction; //The Amount of Metal Scrap you are collecting each turn. 
    public int PowerSupplyTotal; //The Amount of Power output by generators and HQ
    public int PowerSupplyNeeded; //The Amount needed to power everything created. 
    public int PowerSupplyUsed; //The Amount of Power being used 
    public int InfluencePoints; //The Amount of points collected from Flipping tiles. 
    public int ExplorationPointsMax; //The maximum amount of points you have to flip tiles 
    public int ExplorationPointsLeft; //How many points you haveleft this turn. 

    public bool inPowerDeficit; 

    public void AddPoints(ResourcesType _type, int _amount)
    {
        switch (_type)
        {
            case ResourcesType.MetalScrap:
                MetalScrapAmount += _amount; 
                break;
            case ResourcesType.InfluencePoints:
                InfluencePoints += _amount; 
                break;
            case ResourcesType.ExplorationPoints:

                ExplorationPointsLeft += _amount;

                if (ExplorationPointsLeft < 0)
                {
                    ExplorationPointsLeft = 0;
                }
                if (ExplorationPointsLeft > ExplorationPointsMax)
                {
                    ExplorationPointsLeft = ExplorationPointsMax;
                }

                break;
            case ResourcesType.PowerSupplyTotal:
                PowerSupplyTotal += _amount; 
                break;
            case ResourcesType.PowerSupplyUsed:

                PowerSupplyUsed += _amount;

                if(PowerSupplyUsed > PowerSupplyTotal)
                {
                    inPowerDeficit = true; 
                }
                else
                {
                    inPowerDeficit = false;
                }

                break;
            case ResourcesType.Unhexium:
                UnhexiumNodesCaptured += _amount;
                break;
        }
    }



}

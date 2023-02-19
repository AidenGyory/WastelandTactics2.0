using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerInfo : MonoBehaviour
{

    public PlayerSettingsTemplate settings;

    [Header("Resources")]
    public int VictoryPoints; //Score at end of game to see who wins
    public int MetalScrapAmount; //The Amount of MetalScrap Resources you have in Total 
    public int PowerSupplyTotal; //The Amount of Power output by generators and HQ
    public int PowerSupplyUsed; //The Amount of Power being used 
    public int InfluencePoints; //The Amount of points collected from Flipping tiles. 
    public int ExplorationPointsMax; //The maximum amount of points you have to flip tiles 
    public int ExplorationPointsLeft; //How many points you haveleft this turn. 

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
        }
    }



}

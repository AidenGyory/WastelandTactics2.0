using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnHexiumTileScript : TileInfo
{
    public bool captured; 

    public float multiplier; 

    public bool ApplyMultiplier()
    {
        if(captured)
        {
            return true;
        }
        return false;
    }

    public void PrintTileTypeLog()
    {
        //Debug.Log("Tile is an Unhexium Node! Awesome!");
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCacheTileScript : TileInfo 
{
    public bool random; 
    public enum CacheType
    {
        Item = 0, 
        MetalScrap = 1,
        ExplorationPoint = 2,
    }

    public CacheType rewardType;

    public int amount;

    private void Awake()
    {
        if(random)
        rewardType = (CacheType)Random.Range(0, 2); 
    }

    public void PrintTileTypeLog()
    {
        //Debug.Log("Tile is a Reward Cache! Awesome!");
    }
}

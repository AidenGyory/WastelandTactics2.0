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

    public bool rewardGiven;

    public GameObject icon; 

    private void Awake()
    {
        if(random)
        rewardType = (CacheType)Random.Range(0, 2); 
    }

    public void PrintTileTypeLog()
    {
        Debug.Log("Reward Cache"); 
    }

    public void GiveReward(PlayerInfo _player)
    {
        switch (rewardType)
        {
            case CacheType.Item:
                Debug.Log("Relic Given!"); 
                break;
            case CacheType.MetalScrap:
                _player.MetalScrapAmount += 50; 
                break;
            case CacheType.ExplorationPoint:
                _player.ExplorationPointsLeft += 2; 
                break;
        }
        rewardGiven = true;
        icon.SetActive(false); 
    }

}

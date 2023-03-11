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

    [SerializeField] GameObject metalRewardPrefab;
    [SerializeField] float distanceOffset;

    private void Awake()
    {
        if(random)
        rewardType = (CacheType)Random.Range(0, 3); 
    }

    public void GiveReward(PlayerInfo _player)
    {
        switch (rewardType)
        {
            case CacheType.Item:
                Debug.Log("Relic Given!"); 
                break;
            case CacheType.MetalScrap:
                Debug.Log("Give Metal"); 
                GameObject _AddMetalUI = Instantiate(metalRewardPrefab, SelectObjectScript.Instance.CameraScreenCanvas);
                _AddMetalUI.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * distanceOffset;
                _player.MetalScrapAmount += 50; 
                break;
            case CacheType.ExplorationPoint:
                Debug.Log("Give EP"); 
                _player.ExplorationPointsLeft += 2; 
                break;
        }
        rewardGiven = true;
        icon.SetActive(false); 
    }

}

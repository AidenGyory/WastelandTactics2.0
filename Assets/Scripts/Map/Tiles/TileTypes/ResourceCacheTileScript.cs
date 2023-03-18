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
    [SerializeField] GameObject canvas;

    private RelicAbilityScript _relicUI;

    
    private void Awake()
    {
        if(random)
        rewardType = (CacheType)Random.Range(0, 3);
        _relicUI = FindObjectOfType<RelicAbilityScript>(); 
    }

    public void GiveReward(PlayerInfo _player)
    {
        switch (rewardType)
        {
            case CacheType.Item:
                Debug.Log("Relic Given!");
                _relicUI.TurnOnRelicUI(); 
                break;
            case CacheType.MetalScrap:
                Debug.Log("Give Metal");
                float _metalScrap = 50;

                GameObject _AddMetalUI = Instantiate(metalRewardPrefab);
                _AddMetalUI.transform.SetParent(canvas.transform);
                _AddMetalUI.transform.position = canvas.transform.position;
                _AddMetalUI.GetComponent<MetalProductionScript>().productionAmount = Mathf.RoundToInt(_metalScrap);

                GameManager.Instance.currentPlayerTurn.AddMetalScrap(Mathf.RoundToInt(_metalScrap));
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

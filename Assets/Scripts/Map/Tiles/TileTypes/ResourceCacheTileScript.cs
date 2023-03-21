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
    [SerializeField] GameObject relicRewardPrefab; 
    [SerializeField] GameObject exRewardPrefab;
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
        if(!rewardGiven)
        {
            switch (rewardType)
            {
                case CacheType.Item:
                    _relicUI.TurnOnRelicUI(this);
                    break;
                case CacheType.MetalScrap:
                    GiveMetal(50); 
                    break;
                case CacheType.ExplorationPoint:
                    int exPoints = 2;

                    GameObject _AddEPUI = Instantiate(exRewardPrefab);
                    _AddEPUI.transform.SetParent(canvas.transform);
                    _AddEPUI.transform.position = canvas.transform.position;
                    _AddEPUI.GetComponent<MetalProductionScript>().productionAmount = exPoints;

                    GameManager.Instance.currentPlayerTurn.AddExplorationPoints(exPoints);
                    TurnOFFSmoke();
                    break;
            }
            rewardGiven = true;
            
        }
        
    }

    public void TurnOnSmoke()
    {
        if(!rewardGiven)
        {
            icon.SetActive(true);
        }
    }
    public void TurnOFFSmoke()
    {
        icon.SetActive(false);
    }

    public void GiveRelic()
    {
        Debug.Log("Gain Relic");
        GameObject _AddRelic = Instantiate(relicRewardPrefab);
        _AddRelic.transform.SetParent(canvas.transform);
        _AddRelic.transform.position = canvas.transform.position;

        GameManager.Instance.currentPlayerTurn.AddRelic();
        TurnOFFSmoke(); 
    }

    public void GiveMetal(int _metalScrap)
    {

        GameObject _AddMetalUI = Instantiate(metalRewardPrefab);
        _AddMetalUI.transform.SetParent(canvas.transform);
        _AddMetalUI.transform.position = canvas.transform.position;
        _AddMetalUI.GetComponent<MetalProductionScript>().productionAmount = Mathf.RoundToInt(_metalScrap);

        GameManager.Instance.currentPlayerTurn.AddMetalScrap(Mathf.RoundToInt(_metalScrap));
        TurnOFFSmoke();
    }

}

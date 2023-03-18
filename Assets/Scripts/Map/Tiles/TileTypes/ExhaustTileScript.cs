using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustTileScript : TileInfo
{
    [Header("Prefab Element")]
    [SerializeField] GameObject EpRewardUIPrefab;
    [SerializeField] float distanceOffset;
    [SerializeField] GameObject canvas;
    public void AddFlipReward(int _amount)
    {
        GameObject _AddEPUI = Instantiate(EpRewardUIPrefab);
        _AddEPUI.transform.SetParent(canvas.transform);
        _AddEPUI.transform.position = canvas.transform.position;
        _AddEPUI.GetComponent<MetalProductionScript>().productionAmount = _amount;

        GameManager.Instance.currentPlayerTurn.AddExplorationPoints(_amount);
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalMineTileScript : TileInfo
{
    [Header("Prefab Element")]
    [SerializeField] GameObject metalRewardPrefab;
    [SerializeField] float distanceOffset;
    [SerializeField] GameObject canvas;
    public void AddFlipReward(int _amount)
    {
        float _metalScrap = _amount;

        GameObject _AddMetalUI = Instantiate(metalRewardPrefab);
        _AddMetalUI.transform.SetParent(canvas.transform);
        _AddMetalUI.transform.position = canvas.transform.position;
        _AddMetalUI.GetComponent<MetalProductionScript>().productionAmount = Mathf.RoundToInt(_metalScrap);

        GameManager.Instance.currentPlayerTurn.AddMetalScrap(Mathf.RoundToInt(_metalScrap));
    }
}

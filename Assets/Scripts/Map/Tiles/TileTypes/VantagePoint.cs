using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VantagePoint : TileInfo
{
    [Header("Prefab Element")]
    [SerializeField] GameObject AddEpPrefab;
    [SerializeField] float distanceOffset; 
    public void AddExplorationPoint(int _amount)
    {
        GameManager.Instance.currentPlayerTurn.AddExplorationPoints(_amount); 

        GameObject _AddEPUI = Instantiate(AddEpPrefab, SelectObjectScript.Instance.CameraScreenCanvas);

        _AddEPUI.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * distanceOffset; 

        TileManager.instance.FindPlayerOwnedTilesForFlipCheck(Owner); 
    }
}

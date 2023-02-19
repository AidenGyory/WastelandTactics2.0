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
        GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn].AddPoints(ResourcesType.ExplorationPoints, _amount);
        GameObject _AddEPUI = Instantiate(AddEpPrefab, SelectObjectScript.Instance.CameraScreenCanvas);
        _AddEPUI.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * distanceOffset; 
        Debug.Log("Add Exploration Point!!");
        TileManager.instance.FindPlayerOwnedTilesForFlipCheck(Owner); 
    }
}

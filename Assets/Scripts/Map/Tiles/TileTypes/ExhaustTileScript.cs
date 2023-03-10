using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustTileScript : TileInfo
{
    [Header("Prefab Element")]
    [SerializeField] GameObject exhaustUIPrefab;
    [SerializeField] float distanceOffset;
    public void AddExplorationPoint(int _amount)
    {
        GameManager.Instance.currentPlayerTurn.AddExplorationPoints(_amount);

        GameObject _ui = Instantiate(exhaustUIPrefab, SelectObjectScript.Instance.CameraScreenCanvas);

        _ui.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * distanceOffset;
    }
}

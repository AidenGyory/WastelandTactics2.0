using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalMineTileScript : TileInfo
{
    [Header("Prefab Element")]
    [SerializeField] GameObject metalUIPrefab;
    [SerializeField] float distanceOffset;
    public void AddFlipReward(int _amount)
    {
        GameManager.Instance.currentPlayerTurn.AddMetalScrap(_amount);

        GameObject _ui = Instantiate(metalUIPrefab, SelectObjectScript.Instance.CameraScreenCanvas);

        _ui.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * distanceOffset;
    }
}

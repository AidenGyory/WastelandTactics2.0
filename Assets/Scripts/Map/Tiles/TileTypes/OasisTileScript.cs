using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class OasisTileScript : TileInfo
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
    public void FlipTiles()
    {
        for (int i = 0; i < neighbours.Count; i++)
        {

            if (neighbours[i].state != TileState.IsFlipped)
            {
                TileManager.instance.tilesToFlip.Add(neighbours[i]);
            }
        }

        
        if (!TileManager.instance.runFlipTiles)
        {
            TileManager.instance.FlipTilesInList();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class OasisTileScript : TileInfo
{
    [SerializeField] float flipRadius;
    [SerializeField] LayerMask isTiles;

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
    public void FlipTiles()
    {
        for (int i = 0; i < neighbours.Count; i++)
        {

            if (neighbours[i].state != TileState.IsFlipped)
            {
                TileManager.instance.tilesToFlip.Add(neighbours[i]);
            }
        }

        TileManager.instance.UpdateBorders();
        if (!TileManager.instance.runFlipTiles)
        {
            TileManager.instance.FlipTilesInList();
        }
    }
}

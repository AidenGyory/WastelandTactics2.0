using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OasisTileScript : TileInfo
{
    [SerializeField] float flipRadius;
    [SerializeField] LayerMask isTiles;
    List<TileInfo> tilesToFlip = new List<TileInfo>();
    public void FlipTiles()
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].state != TileState.IsFlipped)
            {
                tilesToFlip.Add(neighbours[i]);
            }
        }

        Invoke("WaitAndFlip", 0.1f);
    }

    public void WaitAndFlip()
    {
        if (tilesToFlip.Count < 1)
        {
            GameManager.Instance.currentPlayerTurn.ExplorationPointsLeft = 3;
            TileManager.instance.FindPlayerOwnedTilesForFlipCheck(GameManager.Instance.currentPlayerTurn);

            return;
        }

        int rand = Random.Range(0, tilesToFlip.Count);

        if(tilesToFlip[rand].state != TileState.IsFlipped)
        {
            tilesToFlip[rand].TryToFlipTile();
            tilesToFlip.Remove(tilesToFlip[rand]);
        }

        Invoke("WaitAndFlip", 0.1f);
    }
}

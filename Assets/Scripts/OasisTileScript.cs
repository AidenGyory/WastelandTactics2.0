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
        // find the tiles within unit movement radius 
        Collider[] _tiles = Physics.OverlapSphere(transform.position, flipRadius, isTiles);

        for (int i = 0; i < _tiles.Length; i++)
        {
            TileInfo _info = _tiles[i].GetComponent<TileInfo>();

            if (_info.state != TileState.IsFlipped)
            {
                tilesToFlip.Add(_info);
            }

        }
        Invoke("WaitAndFlip", 0.2f);


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

        tilesToFlip[rand].TryToFlipTile();


        tilesToFlip.Remove(tilesToFlip[rand]);

        Invoke("WaitAndFlip", 0.075f);
    }
}

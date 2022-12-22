using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SandstormTileScript : TileInfo
{
    [SerializeField] bool debug; 
    [SerializeField] float flipRadius;
    [SerializeField] LayerMask isTiles;
    [SerializeField] List<TileInfo> tilesToFlipBack;

    [Header("Prefab Element")]
    [SerializeField] GameObject sandstormUIPrefab;
    [SerializeField] float distanceOffset;

    public void UnFlipTiles()
    {
        GameObject _ui = Instantiate(sandstormUIPrefab, SelectObjectScript.Instance.CameraScreenCanvas);
        _ui.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * distanceOffset;

        //Debug.Log("Uh oh! Sandstorm!");
        //Run Tile Manager Flip Tiles back

        // find the tiles within unit movement radius 
        Collider[] _tiles = Physics.OverlapSphere(transform.position, flipRadius, isTiles);

        for (int i = 0; i < _tiles.Length; i++)
        {
            TileInfo _info = _tiles[i].GetComponent<TileInfo>();

            if(_info.state == TileState.IsFlipped)
            {
                tilesToFlipBack.Add(_info);
            }

        }
        Invoke("WaitAndFlip", 0.2f);
    }

    public void WaitAndFlip()
    {
        if(tilesToFlipBack.Count < 1)
        {
            TileManager.instance.FindPlayerOwnedTilesForFlipCheck(GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn]);
            return; 
        }

        int rand = Random.Range(0, tilesToFlipBack.Count);

        tilesToFlipBack[rand].FlipTileBack();

        
        tilesToFlipBack.Remove(tilesToFlipBack[rand]);

        Invoke("WaitAndFlip", 0.05f);
    }

    

}

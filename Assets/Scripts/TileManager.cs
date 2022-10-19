using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public TileInfo[] tiles;

     
    private void Start()
    {
        Invoke("InitialiseAllTiles", 1f); 
    }
    public void InitialiseAllTiles()
    {
        TileInfo[] obj = FindObjectsOfType<TileInfo>();
        tiles = new TileInfo[obj.Length];
        tiles = obj;
        
        UpdateStructuresOnTiles();
        UpdateUnitsOnTiles();
    }
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void UpdateStructuresOnTiles()
    {
        foreach (TileInfo _tile in tiles)
        {
            if(_tile.state != TileInfo.TileState.isFlipped) { return; }

            if(_tile.structureOnTile == null)
            {
                _tile.AttachStructure();
            }
        }
    }

    void UpdateUnitsOnTiles()
    {
        foreach (TileInfo _tile in tiles)
        {
            if (_tile.state != TileInfo.TileState.isFlipped) { return; }

            if (_tile.unitOnTile == null)
            {
                _tile.AttachUnit();
            }
        }
    }
}

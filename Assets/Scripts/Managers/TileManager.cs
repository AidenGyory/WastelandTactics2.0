using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public TileInfo[] tiles;

    public List<TileInfo> walkableTiles;
    public LayerMask isTiles;

    [Header("Colour Overlays")]
    public Color selected;
    public Color walkable; 
    public Color unwalkable;
    public float brightness;
    [Tooltip("Flash Speed is normally set to 0.3f for flashing white on tiles")]
    public float flashSpeed;

    // Create Instance on Awake 
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
    void Start()
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
    void UpdateStructuresOnTiles()
    {
        foreach (TileInfo _tile in tiles)
        {
            if(_tile.state != TileInfo.TileState.isFlipped) { return; }

            if(_tile.structureOnTile == null)
            {
                _tile.AttachStructureToThisTile();
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
                _tile.AttachUnitToThisTile();
            }
        }
    }
    public void CheckWalkableTiles(Vector3 _position, float _radius)
    {
        // Clear previous list of walkable tiles 
        ClearWalkableTiles();

        // find the tiles within unit movement radius 
        Collider[] _tiles = Physics.OverlapSphere(_position, _radius, isTiles);

        Debug.Log("Check for Walkable Tiles at " + _position + " for " + _radius + "tiles"); 

        for (int i = 0; i < _tiles.Length; i++)
        {
            TileInfo _info = _tiles[i].GetComponent<TileInfo>();

            //Guard for tile state
            if (_info.state != TileInfo.TileState.isFlipped) { return; }

            _info.SetTileWalkableStatus(true);
            _info.ChangeToWalkableMaterial();

            walkableTiles.Add(_info);
        }

    }
    public void ClearWalkableTiles()
    {
        Debug.Log("Clear Walkable Tiles"); 

        foreach(TileInfo _info in walkableTiles)
        {
            _info.SetTileWalkableStatus(false);
            _info.GetComponent<SelectScript>().ClearSelectInfo();
            _info.ChangeMaterialAndScale(_info.GetComponent<SelectScript>().currentSelectState, 0.5f); 
        }

        walkableTiles.Clear();
    }
}

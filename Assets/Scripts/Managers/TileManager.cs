using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class TileManager : MonoBehaviour
{
    // Singleton instance of this class
    public static TileManager instance;

    public float tileSize;
    // Array of all TileInfo objects in the scene
    public List<TileInfo> allTiles;

    // List of TileInfo objects within a given radius of a given position
    public List<TileInfo> tileList;

    // LayerMask used to detect colliders within a given radius
    public LayerMask isTiles;

    // Prefab used to clear tiles during play
    public GameObject clearTilePrefab;

    // Colours used for different types of tiles
    public Color selected;
    public Color walkable;
    public Color unwalkable;
    public Color placeable;
    public Color flippable;

    // Brightness of the flashing white colour
    public float brightness;

    // Speed at which tiles flash white (normally set to 0.3f
    public float flashSpeed;

    public LayerMask isUnit; 

    //List of Tiles that need to be flipped
    public List<TileInfo> tilesToFlip;

    // Initialize the singleton instance on Awake
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ResetIconsOnTiles()
    {
        foreach(TileInfo _tile in allTiles)
        {
            _tile.ShowScanIcon(false); 
        }
    }

    // Populate the tiles array with all TileInfo objects in the scene
    public void InitialiseAllTiles()
    {
        //Clear previous all tiles list 
        allTiles.Clear();

        // Find all objects with TileInfo script. 
        var findTiles = FindObjectsOfType<TileInfo>();

        // Add each tile in findTiles group to allTiles List.  
        foreach (var t in findTiles)
        {
            allTiles.Add(t);
        }
        allTiles[0].EstablishNeighbours();
        
    }

    public void UpdateBorders()
    {
        foreach(TileInfo _tile in allTiles)
        {
            if(_tile.BorderOwner != null)
            {
                _tile.GetComponent<TerritoryBorderScript>().UpdateBorders(); 
            }
        }
    }
    public TileInfo GetClosestTile(Vector3 _target)
    {
        List<TileInfo> _TilesToCheck = SetTileList(_target, 1); 

        if(_TilesToCheck.Count < 1)
        {
            Debug.LogWarning("NO TILES FOUND"); 
            return null; 
        }

        TileInfo _closestTile; 

        if (_TilesToCheck.Count > 1)
        {
            // distance from tile to target tile 
            List<float> _tileDistance = new List<float>();

            for (int i = 0; i < _TilesToCheck.Count - 1; i++)
            {
                _tileDistance.Add(Vector3.Distance(_TilesToCheck[i].transform.position, _target));
            }

            float _minDistance = _tileDistance.Min();
            int _tileindex = _tileDistance.IndexOf(_minDistance);

            _closestTile = _TilesToCheck[_tileindex];
        }
        else
        {
            _closestTile = _TilesToCheck[0];
        }
        
        return _closestTile;
    }

    //Set the owners of the tiles surounding each headquarters on the map 
    public void SetTileOwners()
    {
        //Find all headquarters on the map. 
        var hq = FindObjectsOfType<HeadQuarters>();

        // run the "Update tile Ownership" function from all headquarters
        foreach (var headQuarters in hq)
        {
            //headQuarters.UpdateTileOwnership();
        }

    }

    // Set up the tileList with TileInfo objects within a given radius of a given position
    public List<TileInfo> SetTileList(Vector3 position, int radiusInTiles)
    {
        // execute Clear Functions for all tiles in tileList; 
        if (tileList.Count > 0)
        {
            ClearTilesList();
        }

        //Check Radius is more than 0 
        float _radius = radiusInTiles * tileSize; 
        if(_radius <= 0)
        {
            _radius = 0.1f;  
        }

        // Find all colliders within the given radius and isTiles layermask
        Collider[] tiles = Physics.OverlapSphere(position, _radius, isTiles);

        for (int i = 0; i < tiles.Length; i++)
        {
            TileInfo _info = tiles[i].GetComponent<TileInfo>();
            tileList.Add(_info);
        }

        return tileList;
    }

    // Clear the tileList
    public void ClearTilesList()
    {
        // Clear the select info for each TileInfo object in the list
        foreach (TileInfo _info in tileList)
        {
            _info.GetComponent<SelectScript>().ClearSelectInfo();
        }

        // Clear the list itself
        tileList.Clear();
    }

    // Flip tiles within a given radius of a given position
    public void FlipTiles(List<TileInfo> _tilesToFlip)
    {
        // Set up the tileList with TileInfo objects within the given radius of the given position
        ClearTilesList();

        tileList = _tilesToFlip; 

        // Wait 0.2 seconds before flipping the first tile
        Invoke(nameof(WaitAndFlip), 0.2f);
    }

    public void SetTilesAsMoveable(TileInfo _start, List<TileInfo> _tiles, bool _ignoreTerrainType)
    {
        // Reset Modeable State on all tiles
        ClearMoveableStateOnAllTiles();

        // Reset Flipstate on all tiles 
        ClearCanFlipStateOnAllTiles(); 

        // Check all tiles in _tiles list 
        for (int i = 0; i < _tiles.Count; i++)
        {
            // Set reference to tile info 
            TileInfo _tile = _tiles[i];

            // Set unwalkable if occupied 
            if (_tile.isOccupied)
            {
                _tile.SetTileToUnwalkable();
            }
            // if not occupied check if we are ignoring terrain like mountains and flipped tiles 
            else if(_ignoreTerrainType)
            {
                _tile.SetTileToWalkable();
            }
            // If not ignoring terrain check if tile is flipped 
            else if (_tile.state == TileInfo.TileState.IsFlipped)
            {
                //if tile is flipped check if unwalkable is true 
                if (_tile.unwalkable)
                {
                    _tile.SetTileToUnwalkable();
                }
                //If not unwalkable then set as walkable
                else
                {
                    _tile.SetTileToWalkable();
                }
            }

            
        }

        // CAN CAUSE LAG WILL PATHFIND ON ALL REMAINING TILES 
        for (int j = 0; j < _tiles.Count; j++)
        {
            TileInfo _tile = _tiles[j]; 

            if (_tile.state == TileInfo.TileState.walkable)
            {
                if (PathfindingManager.Instance.GetPath(_start, _tile, _ignoreTerrainType) == null)
                {
                    _tile.SetTileToUnwalkable();
                }
            }
        }
    }

    // Wait a short period of time before flipping a random tile in the tileList
    public void WaitAndFlip()
    {
        if (tileList.Count < 1)
        {
            return;
        }

        // Choose a random tile from the list
        int rand = Random.Range(0, tileList.Count);

        // Flip the chosen tile
        tileList[rand].DOFlip();

        // Remove the flipped tile from the list
        tileList.Remove(tileList[rand]);

        //loop around
        Invoke(nameof(WaitAndFlip), 0.05f);
    }

    // Find tiles close to flipped tiles and set their canFlip property to true
    public void FindAdjacentFlippableTiles(Vector3 _position, int _radiusInTiles)
    {
        // Set up the tileList with TileInfo objects within the given radius of the given position
        List<TileInfo> _tilesToSetToCanFlip = SetTileList(_position, _radiusInTiles);

        // Iterate through the tileList
        foreach (TileInfo _tile in _tilesToSetToCanFlip)
        {
            // Check if the tile has already been flipped
            if (_tile.state == TileInfo.TileState.CannotFlip)
            {
                //Set tile state to can flip
                _tile.SetasCanFlip();

            }
        }
    }

    //method to find all tiles that are owned by the current player
    public void FindPlayerOwnedTilesForFlipCheck(PlayerInfo _currentPlayer)
    {
        //Clear the state of all "CanFlip" tiles
        ClearCanFlipStateOnAllTiles();

        if (_currentPlayer.ExplorationPointsLeft > 0)
        {

            //Create local List of Ownder Tiles 
            List<TileInfo> _ownedTiles = new List<TileInfo>();

            //iterate through list of tiles to find owned tiles 
            for (int i = 0; i < allTiles.Count - 1; i++)
            {
                if (allTiles[i].GetComponent<TileInfo>().Owner == _currentPlayer)
                {
                    _ownedTiles.Add(allTiles[i]);
                    //Debug.Log("Tile: " + allTiles[i] + " Added to list");
                }
            }

            //iterate through the list of tiles on on the gameboard
            for (int i = 0; i < _ownedTiles.Count; i++)
            {
                if (_ownedTiles[i].Checkable)
                {
                    FindAdjacentFlippableTiles(_ownedTiles[i].transform.position, 1);
                }
            }
        }
    }
    public void ClearCanFlipStateOnAllTiles()
    {
        ClearMoveableStateOnAllTiles();
        //Iterate through all tiles in the list of tiles 
        foreach (TileInfo _tile in allTiles)
        {
            //Guard clause for call tiles that are not canFlip tiles. 
            if (_tile.state == TileInfo.TileState.CanFlip)
            {
                _tile.state = TileInfo.TileState.CannotFlip;
                _tile.UnselectTile();
            }
        }
    }

    public void ClearPlaceableStateOnAllTiles()
    {
        //Iterate through all tiles in the list of tiles 
        foreach (TileInfo _tile in allTiles)
        {
            //Guard clause for call tiles that are not canFlip tiles. 
            if (_tile.state == TileInfo.TileState.CanPlace)
            {
                _tile.state = TileInfo.TileState.IsFlipped;
                _tile.UnselectTile();
            }
        }
    }

    public void ClearMoveableStateOnAllTiles()
    {
        //Iterate through all tiles in the list of tiles 
        foreach (TileInfo _tile in allTiles)
        { 
            if (_tile.state == TileInfo.TileState.walkable || _tile.state == TileInfo.TileState.unwalkable)
            {
                if(_tile.Owner == null)
                {
                    _tile.state = TileInfo.TileState.CannotFlip;
                }
                else
                {
                    _tile.state = TileInfo.TileState.IsFlipped; 
                }
                
                _tile.UnselectTile();
            }
        }
    }

    public void SetBorderTileOwnership()
    {
        foreach(TileInfo _tile in allTiles)
        {
            _tile.BorderOwner = null; 
        }

        StructureInfo[] structures = FindObjectsOfType<StructureInfo>();

        foreach (StructureInfo _building in structures)
        {
            _building.UpdateBorder(); 
        }

        UpdateBorders();
    }

    public bool runFlipTiles; 
    public void FlipTilesInList()
    {

        if (tilesToFlip.Count < 1) 
        {
            FindPlayerOwnedTilesForFlipCheck(GameManager.Instance.currentPlayerTurn);
            runFlipTiles = false; 
        }
        else
        {
            if(tilesToFlip[0].state == TileInfo.TileState.IsFlipped)
            {
                tilesToFlip.RemoveAt(0);
                runFlipTiles = true;
                FlipTilesInList(); 
            }
            else
            {
                tilesToFlip[0].FlipTile();
                tilesToFlip.RemoveAt(0);
                runFlipTiles = true;
                Invoke("FlipTilesInList", 0.2f);
            }
        }

        
    }
}

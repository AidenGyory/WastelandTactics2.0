using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

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

    private void Start()
    {
        // Load Game Order
        Invoke(nameof(InitialiseAllTiles), 1f);
        Invoke(nameof(SetTileOwners), 1.1f);
        Invoke(nameof(StartGame), 2f);
    }
    public void StartGame()
    {
        // Officially make the game start and playable. 
        GameManager.Instance.StartGame();
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
    }

    public TileInfo GetTile(Vector3 _targetPosition)
    {
        // Create a ray that points downward from the target position
        Ray ray = new Ray(_targetPosition, Vector3.down * Mathf.Infinity);

        // Perform a raycast from the target position downward
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            // Check if the raycast hit an object with the TileInfo component
            TileInfo tileInfo = hitInfo.collider.GetComponent<TileInfo>();
            if (tileInfo != null)
            {
                return tileInfo;
            }
        }
        return null;
    }

    //Set the owners of the tiles surounding each headquarters on the map 
    public void SetTileOwners()
    {
        //Find all headquarters on the map. 
        var hq = FindObjectsOfType<HeadQuarters>();

        // run the "Update tile Ownership" function from all headquarters
        foreach (var headQuarters in hq)
        {
            headQuarters.UpdateTileOwnership();
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

    public void SetTilesAsMoveable(List<TileInfo> _tiles, bool _ignoreMovementCost)
    {
        foreach (TileInfo _tile in _tiles)
        {
             

            if(!_tile.isEmpty)
            {
                _tile.state = TileInfo.TileState.unwalkable;
                foreach (Renderer _model in _tile.modelMaterials)
                {
                    _model.material.DOColor(TileManager.instance.unwalkable, 0.3f);
                }
                return;
            }

            if (_tile.state == TileInfo.TileState.IsFlipped)
            {
                _tile.state = TileInfo.TileState.walkable;
                foreach (Renderer _model in _tile.modelMaterials)
                {
                    _model.material.DOColor(TileManager.instance.walkable, 0.3f);
                }
            }
            else
            {
                if(_ignoreMovementCost)
                {
                    _tile.state = TileInfo.TileState.walkable;
                    foreach (Renderer _model in _tile.modelMaterials)
                    {
                        _model.material.DOColor(TileManager.instance.walkable, 0.3f);
                    }
                }
                else
                {
                    _tile.state = TileInfo.TileState.unwalkable;
                    foreach (Renderer _model in _tile.modelMaterials)
                    {
                        _model.material.DOColor(TileManager.instance.unwalkable, 0.3f);
                    }
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

        //Create a local list of vector 3 positions 
        List<Vector3> _tilePositions = new List<Vector3>();

        //iterate through the list of tiles on on the gameboard
        for (int i = 0; i < allTiles.Count-1; i++)
        {
            //check if the tile belongs to the current player
            if (allTiles[i].GetComponent<TileInfo>().Owner == _currentPlayer)
            {
                //add the tile to the local list 
                _tilePositions.Add(allTiles[i].transform.position);
            }
        }

        //Once the local list is populated 
        for (int i = 0; i < _tilePositions.Count; i++)
        {
            FindAdjacentFlippableTiles(_tilePositions[i], 1);
        }

    }
    public void ClearCanFlipStateOnAllTiles()
    {
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
}

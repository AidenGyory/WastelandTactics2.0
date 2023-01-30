using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class PathfindingManager : MonoBehaviour
{
    public static PathfindingManager Instance;


    private void Awake()
    {
        Instance = this; 
    }

    public List<TileInfo> GetPath(TileInfo _start, TileInfo _end)
    {

        List<TileInfo> _tilepath = new List<TileInfo>();
        List<PathfindingTile> _activeTiles = new List<PathfindingTile>();
        List<TileInfo> _visitedTiles = new List<TileInfo>();

        PathfindingTile _startTile = new PathfindingTile(_start,null,_end);
        _activeTiles.Add(_startTile);

        while (_activeTiles.Any())
        {
            PathfindingTile _currentTile = _activeTiles.OrderBy(x=>x.distance).First();
            if( _currentTile.tile == _end)
            {
                PathfindingTile _checkTile = _currentTile;
                while (_checkTile.tileParent != null)
                {
                    _tilepath.Add(_checkTile.tile);
                    _checkTile = _checkTile.tileParent; 
                }
                _tilepath.Add(_start);
                _tilepath.Reverse();

                return _tilepath; 
            }
            _visitedTiles.Add(_currentTile.tile);
            _activeTiles.Remove(_currentTile);

            List<TileInfo> _walkableTiles = TileManager.instance.SetTileList(_currentTile.tile.transform.position, 1);

            foreach(TileInfo _tile in _walkableTiles)
            {
                if (_tile.state == TileInfo.TileState.unwalkable)
                {
                    _visitedTiles.Add(_tile);
                    continue; 
                }
                if(_visitedTiles.Contains(_tile))
                {
                    continue; 
                }
                if(_activeTiles.Any(x=>x.tile == _tile))
                {
                    PathfindingTile _existingTile = _activeTiles.First(x => x.tile == _tile); 
                    if(_existingTile.distance > _currentTile.distance)
                    {
                        _activeTiles.Remove(_existingTile);
                        _activeTiles.Add(new PathfindingTile(_tile, _currentTile, _end)); 


                    }
                }
                else
                {
                    _activeTiles.Add(new PathfindingTile(_tile,_currentTile,_end));
                }
            }
        }
        return null; 
    }

    public class PathfindingTile
    {
        public TileInfo tile;
        public PathfindingTile tileParent;
        public float distance;

        public PathfindingTile(TileInfo tile, PathfindingTile tileParent, TileInfo _targetTile)
        {
            this.tile = tile;
            this.tileParent = tileParent;

            SetDistance(_targetTile); 
        }

        public void SetDistance(TileInfo _targetTile)
        {
            distance = Vector3.Distance(tile.transform.position,_targetTile.transform.position);
        }
    }
}

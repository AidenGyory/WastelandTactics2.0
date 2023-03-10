using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("List of Hex Tiles")]
    [SerializeField] GameObject _randomTilePrefab;

    [Header("Default Map Settings")]
    [Tooltip("This data is only used if no Map Profile is referenced.")]
    [SerializeField] Vector2 _mapSize;
    [Space]
    [SerializeField] float _tileXOffset;
    [SerializeField] float _tileZOffset;

    public void CreateHexTileMap(SetupGameManager _setup, MapProfileTemplateSO _mapProfile)
    {
        if (_mapProfile != null)
        {
            _mapSize = _mapProfile.mapSize;
            _tileXOffset = _mapProfile.tileXOffset;
            _tileZOffset = _mapProfile.tileZOffset;
        }

        //for each number on the X Axis (width) 
        for (int x = 0; x <= _mapSize.x; x++)
        {
            //for each number on the Z Axis (height) 
            for (int z = 0; z <= _mapSize.y; z++)
            {
                GameObject _tile = Instantiate(_randomTilePrefab);

                if (z % 2 == 0)
                {
                    //If even number then offset in a line
                    _tile.transform.position = new Vector3(transform.position.x + x * _tileXOffset, transform.position.y, transform.position.z + z * _tileZOffset);
                }
                else
                {
                    // if odd number then offset half the width to slot in the middle
                    _tile.transform.position = new Vector3(transform.position.x + x * _tileXOffset + _tileXOffset / 2, transform.position.y, transform.position.z + z * _tileZOffset);
                }
                SetTileParent(_tile, x, z);

            }
        }
        _setup.TileMapCompleted(); 
    }

    // Set the tile parent to make clean 
    void SetTileParent(GameObject _tile, int x, int z)
    {
        _tile.transform.parent = transform;
        _tile.name = "HexTile: " + x.ToString() + ", " + z.ToString();
        
    }
}
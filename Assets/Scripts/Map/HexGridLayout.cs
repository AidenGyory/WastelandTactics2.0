using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("List of Hex Tiles")]
    [SerializeField] GameObject hexTilePrefab;

    [Header("Map Size")]
    [SerializeField] Vector2 mapSize;

    [SerializeField] float _tileXOffset;
    [SerializeField] float _tileZOffset;

    void Start()
    {
        CreateHexTileMap();
    }


    void CreateHexTileMap()
    {
        //for each number on the X Axis (width) 
        for (int x = 0; x <= mapSize.x; x++)
        {
            //for each number on the Z Axis (height) 
            for (int z = 0; z <= mapSize.y; z++)
            {
                GameObject _tile = Instantiate(hexTilePrefab);

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
    }

    // Set the tile parent to make clean 
    void SetTileParent(GameObject _tile, int x, int z)
    {
        _tile.transform.parent = transform;
        _tile.name = "HexTile: " + x.ToString() + ", " + z.ToString();
        
    }
}
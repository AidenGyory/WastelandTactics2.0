using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearTileScript : TileInfo 
{
    [SerializeField] Material[] emptyMaterials;
    [SerializeField] Renderer tileMaterial; 
    private void Awake()
    {
        tileMaterial.material = emptyMaterials[Random.Range(0,emptyMaterials.Length)];
    }
    public void PrintTileTypeLog()
    {
        //Debug.Log("Tile is Empty"); 
    }
}

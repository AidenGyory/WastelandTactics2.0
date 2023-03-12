using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutUnit : UnitInfo
{
    [SerializeField] int scanRadiusInTiles;
    [SerializeField] bool constantScan; 


    //Invoked by Unity Event 
    public void UpdateMaterials()
    {
        for (int i = 0; i < models.Length; i++)
        {
            if (owner.settings.ScoutUnitMaterial != null)
            {
                Renderer[] _modelRenderers = GetComponentsInChildren<Renderer>();

                foreach (Renderer _renderer in _modelRenderers)
                {
                    _renderer.material = owner.settings.ScoutUnitMaterial; 
                }
            }
            else
                models[i].GetComponentInChildren<Renderer>().material = owner.settings.baseMaterial;
        }
    }

    private void LateUpdate()
    {
        if(occuipedTile.type == TileInfo.TileType.Reward && occuipedTile.state == TileInfo.TileState.IsFlipped)
        {
            if(!occuipedTile.GetComponent<ResourceCacheTileScript>().rewardGiven)
            {
                occuipedTile.GetComponent<ResourceCacheTileScript>().GiveReward(owner); 
            }
        }

        if (constantScan)
        {
            if(GameManager.Instance.currentPlayerTurn == owner)
            {
                List<TileInfo> _tilesScanned = TileManager.instance.SetTileList(transform.position, scanRadiusInTiles);
                foreach (TileInfo _tile in _tilesScanned)
                {
                    if(!_tile.GetComponent<ScanTileScript>().scanned)
                    {
                        _tile.GetComponent<ScanTileScript>().TurnOnScanLayer(); 
                    }
                }

                
            }
        }
    }




}

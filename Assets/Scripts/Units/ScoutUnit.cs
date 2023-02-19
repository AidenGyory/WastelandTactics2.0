using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutUnit : UnitInfo
{
    bool _areaScanned; 
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

    //Invoked by Unity Event "PlayAction" 
    public void ScanArea()
    {
        if(!_areaScanned)
        {
            _areaScanned = true;
            Debug.Log("Dropped a Beacon");
            //ADD SCAN FOR TILES 

            List<TileInfo> _scannedTiles = TileManager.instance.SetTileList(occuipedTile.transform.position, 1);

            for (int i = 0; i < _scannedTiles.Count; i++)
            {
                if (_scannedTiles[i].state != TileInfo.TileState.IsFlipped)
                {
                    _scannedTiles[i].ShowScanIcon(true); 
                }
            }
        }
        else
        {
            Debug.Log("Already Scanned!!"); 
        }


    }

    public void ReplenishScanAbility()
    {
        _areaScanned = false; 
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
    }


}

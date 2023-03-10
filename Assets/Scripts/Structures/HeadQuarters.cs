using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeadQuarters : StructureInfo
{
    public int MetalOutput;
    public GameObject scoutPrefab;
    [SerializeField] bool freeScout;

    [SerializeField] GameObject metalRewardPrefab;
    [SerializeField] float distanceOffset;

    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.HQMaterial != null)
            {
                Renderer[] _modelRenderers = GetComponentsInChildren<Renderer>();

                foreach (Renderer _renderer in _modelRenderers)
                {
                    _renderer.material = owner.settings.HQMaterial;
                }
            }
        }
    }

    //used to update the "player info" object to match the headquarters building for turn start camera focus
    public void UpdatePlayerLocation()
    {
        //Find the correct Player owner by iterating through the list 
        for (int i = 0; i < GameManager.Instance.players.Length; i++)
        {
            if (GameManager.Instance.players[i] == owner)
            {
                //set the position of the player info object to the headquarters position. 
                GameManager.Instance.players[i].transform.position = transform.position; 
            }
        }
    }


    public void OpenRadialMenu()
    {
        if(SelectObjectScript.Instance.canSelect)
        {
            
            SelectObjectScript.Instance.CameraScreenCanvas.GetComponent<RadialMenuForStructures>().OpenRadialMenu();
            TileManager.instance.ClearCanFlipStateOnAllTiles();
            SelectObjectScript.Instance.canSelect = false;
             
        } 
    }

    public void CloseRadialMenu()
    {
        SelectObjectScript.Instance.CameraScreenCanvas.GetComponent<RadialMenuForStructures>().CloseRadialMenu();
        SelectObjectScript.Instance.SetModeToSelect(); 
    }

    public void ProduceMetal()
    {
        GameObject _AddMetalUI = Instantiate(metalRewardPrefab);
        _AddMetalUI.transform.position = transform.position + Vector3.up * distanceOffset; 
        owner.MetalScrapAmount += MetalOutput; 
    }

    public void FreeScout()
    {
        if(freeScout)
        {
            List<TileInfo> _scoutTiles = new List<TileInfo>(); 

            foreach(TileInfo _tile in occupiedTile.neighbours)
            {
                if(!_tile.isOccupied)
                {
                    _scoutTiles.Add(_tile); 
                }
            }

            TileInfo _spawnTile = _scoutTiles[Random.Range(0, _scoutTiles.Count)];

            GameObject _scout = Instantiate(scoutPrefab);
            _scout.transform.position = _spawnTile.transform.position;
            _scout.GetComponent<ScoutUnit>().occuipedTile = _spawnTile;
            _scout.GetComponent<ScoutUnit>().owner = owner;
            _scout.GetComponent<ScoutUnit>().UpdatePlayerDetails(); 
            _spawnTile.isOccupied = true;
            freeScout = false; 
        }
    }

    public void SetTileOwnership()
    {
        Collider[] _tiles = Physics.OverlapSphere(occupiedTile.transform.position,sightRangeInTiles);

        foreach (Collider _tile in _tiles)
        {
            if(_tile.GetComponent<TileInfo>() != null)
            {
                if(_tile.GetComponent<TileInfo>().state == TileInfo.TileState.IsFlipped)
                {
                    _tile.GetComponent<TileInfo>().Owner = owner;
                    UpdateBorder();
                }

            }
        }
    }

}

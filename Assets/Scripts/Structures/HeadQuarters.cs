using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadQuarters : StructureInfo
{
    
    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.HQMaterial[i] != null)
            {
                modelMaterials[i].material = owner.settings.HQMaterial[i]; 
            }
            else
            modelMaterials[i].material = owner.settings.baseMaterial; 
        }
    }

    //used to update the "player info" object to match the headquarters building for turn start camera focus
    public void UpdatePlayerLocation()
    {
        //Find the correct Player owner by iterating through the list 
        for (int i = 0; i < GameManager.Instance.playerInfo.Length; i++)
        {
            if (GameManager.Instance.playerInfo[i] == owner)
            {
                //set the position of the player info object to the headquarters position. 
                GameManager.Instance.playerInfo[i].transform.position = transform.position; 
            }
        }
    }

    public void UpdateTileOwnership()
    {
        List<TileInfo> _tileList = TileManager.instance.SetTileList(transform.position, 0);

        if (_tileList.Count > 0) 
        {
            _tileList[0].isOccupied = true;
            _tileList.Clear();
        }

        //create a list of tiles 
        _tileList = TileManager.instance.SetTileList(transform.position,sightRangeInTiles);

        //check if there are tiles in the list 
        if (_tileList.Count < 1) { return;  }

        //iterate through the list of tiles 
        foreach (TileInfo _tile in _tileList)
        {
            //check if the tile is already flipped 
            if (_tile.state == TileInfo.TileState.IsFlipped)
            {
                //set the owner of the tile to match the owner of the building.  
                _tile.Owner = owner;
            }

        }
    }
    public void UpdateTileOwnership(List<TileInfo> tilesToSet)
    {
        //create a tile list of the tiles surrounding the Headquarters
        TileManager.instance.SetTileList(transform.position, 1);

        //check if there are tiles in the list 
        if (tilesToSet.Count < 1) { return; }

        //iterate through the list of tiles 
        foreach (TileInfo _tile in tilesToSet)
        {
            //check if the tile is already flipped 
            if (_tile.state == TileInfo.TileState.IsFlipped)
            {
                //set the owner of the tile to match the owner of the building.  
                _tile.Owner = owner;
            }

        }
    }

    public void OpenRadialMenu()
    {
        SelectObjectScript.Instance.CameraScreenCanvas.GetComponent<RadialMenuController>().OpenRadialMenu();
        SelectObjectScript.Instance.canSelect = false; 
    }

    public void CloseRadialMenu()
    {
        SelectObjectScript.Instance.CameraScreenCanvas.GetComponent<RadialMenuController>().CloseRadialMenu();
        SelectObjectScript.Instance.canSelect = true;
    }

}

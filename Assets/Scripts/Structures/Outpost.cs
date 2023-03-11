using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileInfo;

public class Outpost : StructureInfo
{
    List<TileInfo> tilesToFlip = new List<TileInfo>(); 
    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.OutpostMaterial != null)
            {
                Renderer[] _modelRenderers = GetComponentsInChildren<Renderer>();

                foreach (Renderer _renderer in _modelRenderers)
                {
                    _renderer.material = owner.settings.OutpostMaterial;
                }
            }
            else
                modelMaterials[i].material = owner.settings.baseMaterial;
        }
    }

    public void FlipTiles()
    {
        for (int i = 0; i < occupiedTile.neighbours.Count; i++)
        {
            occupiedTile.neighbours[i].Owner = owner; 
            occupiedTile.neighbours[i].BorderOwner = owner;

            if (occupiedTile.neighbours[i].state == TileState.IsFlipped)
            {

            }
            else
            {
                TileManager.instance.tilesToFlip.Add(occupiedTile.neighbours[i]);
            }
        }
        TileManager.instance.UpdateBorders();
        if(!TileManager.instance.runFlipTiles)
        {
            TileManager.instance.FlipTilesInList();
        }
    }

    public void OpenRadialMenu()
    {
        if (SelectObjectScript.Instance.canSelect)
        {

            SelectObjectScript.Instance.CameraScreenCanvas.GetComponent<RadialMenuForUnits>().OpenRadialMenu();
            TileManager.instance.ClearCanFlipStateOnAllTiles();
            SelectObjectScript.Instance.canSelect = false;

        }
    }

    public void CloseRadialMenu()
    {
        SelectObjectScript.Instance.CameraScreenCanvas.GetComponent<RadialMenuForUnits>().CloseRadialMenu();
        SelectObjectScript.Instance.SetModeToSelect();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outpost : StructureInfo
{
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


            if(occupiedTile.neighbours[i].state != TileInfo.TileState.IsFlipped)
            {
                if(occupiedTile.neighbours[i].type == TileInfo.TileType.Sandstorm)
                {
                    occupiedTile.neighbours[i].GetComponent<SandstormTileScript>().ClearSandstorm(); 
                }
                occupiedTile.neighbours[i].TryToFlipTile(); 
            }
        }
        
    }
}

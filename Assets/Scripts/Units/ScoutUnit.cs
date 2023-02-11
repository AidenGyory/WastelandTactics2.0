using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutUnit : UnitInfo
{
    

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

    public void DropBeacon()
    {
        if(occuipedTile.state != TileInfo.TileState.IsFlipped)
        {
            if(occuipedTile.type == TileInfo.TileType.Sandstorm && !occuipedTile.isFlagged)
            {
                occuipedTile.ToggleFlagState(); 
            }
        }

        for (int i = 0; i < occuipedTile.neighbours.Count; i++)
        {
            if (occuipedTile.neighbours[i].state != TileInfo.TileState.IsFlipped)
            {
                if (occuipedTile.neighbours[i].type == TileInfo.TileType.Sandstorm && !occuipedTile.neighbours[i].isFlagged)
                {
                    occuipedTile.neighbours[i].ToggleFlagState();
                }
            }
        }
    }

    
}

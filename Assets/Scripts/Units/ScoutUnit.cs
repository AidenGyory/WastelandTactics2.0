using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutUnit : UnitInfo
{
    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.ScoutUnitMaterial[i] != null)
            {
                modelMaterials[i].material = owner.settings.ScoutUnitMaterial[i];
            }
            else
                modelMaterials[i].material = owner.settings.baseMaterial;
        }
    }

    public void CheckMovement()
    {
        SelectObjectScript.Instance.mode = SelectObjectScript.PointerMode.MoveMode; 
        TileManager.instance.ClearCanFlipStateOnAllTiles();
        List<TileInfo> _moveableTiles = TileManager.instance.SetTileList(transform.position, currentMovementTiles);
        TileManager.instance.SetTilesAsMoveable(_moveableTiles,true); 
    }
}

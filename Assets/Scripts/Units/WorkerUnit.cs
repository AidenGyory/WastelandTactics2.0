using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerUnit : UnitInfo
{

    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.WorkerUnitMaterial[i] != null)
            {
                modelMaterials[i].material = owner.settings.WorkerUnitMaterial[i];
            }
            else
                modelMaterials[i].material = owner.settings.baseMaterial;
        }
    }

    public void CheckMovement()
    {
        //Set Selection Mode (Move unit Mode) 
        SelectObjectScript.Instance.mode = SelectObjectScript.PointerMode.MoveMode;

        //Create a list of all "moveable" tiles. 
        List<TileInfo> _moveableTiles = TileManager.instance.SetTileList(transform.position, currentMovementTiles);

        // set tiles to moveable from list ignoring terrain
        TileManager.instance.SetTilesAsMoveable(_moveableTiles, canFly);

    }
}

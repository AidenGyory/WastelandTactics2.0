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
        // find the tiles within unit movement radius 
        Collider[] _tiles = Physics.OverlapSphere(occupiedTile.transform.position, 1, TileManager.instance.isTiles);

        for (int i = 0; i < _tiles.Length; i++)
        {
            TileInfo _info = _tiles[i].GetComponent<TileInfo>();

            if (_info.state != TileState.IsFlipped)
            {
                tilesToFlip.Add(_info);
            }

        }
        Invoke("WaitAndFlip", 0.2f);


    }

    public void WaitAndFlip()
    {
        if (tilesToFlip.Count < 1)
        {
            TileManager.instance.FindPlayerOwnedTilesForFlipCheck(GameManager.Instance.currentPlayerTurn);
            return;
        }

        int rand = Random.Range(0, tilesToFlip.Count);

        tilesToFlip[rand].TryToFlipTile();

        tilesToFlip.Remove(tilesToFlip[rand]);

        Invoke("WaitAndFlip", 0.075f);
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

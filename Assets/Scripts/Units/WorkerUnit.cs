using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerUnit : UnitInfo
{

    public enum StructureType
    {
        Outpost,
        Factory,
        PowerCell,
        Research,
        Headquarters
    }

    public GameObject[] PlaceableStructures; 

    public void UpdateMaterials()
    {
        for (int i = 0; i < models.Length; i++)
        {
            if (owner.settings.WorkerUnitMaterial != null)
            {
                Renderer[] _modelRenderers = GetComponentsInChildren<Renderer>();

                foreach (Renderer _renderer in _modelRenderers)
                {
                    _renderer.material = owner.settings.WorkerUnitMaterial;
                }
            }
            else
                models[i].GetComponentInChildren<Renderer>().material = owner.settings.baseMaterial;
        }
    }

    public bool CheckIfSuitablePlaceForStructure(StructureType index)
    {
        bool _suitable = false;

        TileInfo.TileType _tileType = occuipedTile.type;

        //Debug.Log("Occupied Tile is " + _tileType);

        switch (index)
        {
            case StructureType.Outpost: //Outpost can be placed on anything 
                {
                    if (_tileType != TileInfo.TileType.Mountain)
                    {
                        _suitable = true;
                    }
                }
                break;
            case StructureType.Factory: //Factory can be placed on Metal or Unhexium nodes 
                {
                    if (_tileType == TileInfo.TileType.MetalMine || _tileType == TileInfo.TileType.Unhexium)
                    {
                        _suitable = true;
                    }
                }
                break;
            case StructureType.PowerCell: //Powercell can be placed on Unhexium nodes 
                {
                    if (_tileType == TileInfo.TileType.Unhexium)
                    {
                        _suitable = true;
                    }
                }
                break;
            case StructureType.Research: //Research can be placed anywhere 
                {
                    if (_tileType != TileInfo.TileType.Mountain)
                    {
                        _suitable = true;
                    }
                }
                break;
            case StructureType.Headquarters: //Headquarters can be placed anywhere 
                {
                    if (_tileType != TileInfo.TileType.Mountain)
                    {
                        _suitable = true;
                    }
                }
                break;
        }
        //Debug.Log("Location is suitable?: " + _suitable); 
        return _suitable; 
    }



    public void OpenRadialMenu()
    {
        SelectObjectScript.Instance.CameraScreenCanvas.GetComponent<RadialBuildMenu>().OpenRadialMenu();
        SelectObjectScript.Instance.CameraScreenCanvas.GetComponent<RadialBuildMenu>().SelectedWorkerUnit = this.gameObject;
        SelectObjectScript.Instance.canSelect = false;
    }

    public void CloseRadialMenu()
    {
        SelectObjectScript.Instance.CameraScreenCanvas.GetComponent<RadialBuildMenu>().SelectedWorkerUnit = null;
        SelectObjectScript.Instance.CameraScreenCanvas.GetComponent<RadialBuildMenu>().CloseRadialMenu();
        SelectObjectScript.Instance.canSelect = true;
    }
}

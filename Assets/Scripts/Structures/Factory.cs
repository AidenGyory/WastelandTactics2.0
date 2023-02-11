using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Factory : StructureInfo
{
    public enum ResourceType
    {
        Metal,
        UnHexium,
    }
    public bool active;

    public ResourceType resource;

    public int resourceAmount; 

    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.FactoryMaterial != null)
            {
                Renderer[] _modelRenderers = GetComponentsInChildren<Renderer>();

                foreach (Renderer _renderer in _modelRenderers)
                {
                    _renderer.material = owner.settings.FactoryMaterial;
                }
            }
            else
                modelMaterials[i].material = owner.settings.baseMaterial;
        }

        if(occupiedTile.type == TileInfo.TileType.Unhexium)
        {
            CaptureNode(); 
        }
    }

    public void CaptureNode()
    {
        owner.AddPoints(ResourcesType.Unhexium, 1);
        resource = ResourceType.UnHexium; 
    }
}

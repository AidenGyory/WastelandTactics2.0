using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : StructureInfo
{
    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.PowerMaterial != null)
            {
                Renderer[] _modelRenderers = GetComponentsInChildren<Renderer>();

                foreach (Renderer _renderer in _modelRenderers)
                {
                    _renderer.material = owner.settings.PowerMaterial;
                }
            }
            else
                modelMaterials[i].material = owner.settings.baseMaterial;
        }
    }
}

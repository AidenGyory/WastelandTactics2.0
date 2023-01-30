using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : StructureInfo
{
    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.FactoryMaterial[i] != null)
            {
                modelMaterials[i].material = owner.settings.FactoryMaterial[i];
            }
            else
                modelMaterials[i].material = owner.settings.baseMaterial;
        }
    }
}

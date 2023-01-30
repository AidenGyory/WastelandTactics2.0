using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : StructureInfo
{
    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.PowerMaterial[i] != null)
            {
                modelMaterials[i].material = owner.settings.PowerMaterial[i];
            }
            else
                modelMaterials[i].material = owner.settings.baseMaterial;
        }
    }
}

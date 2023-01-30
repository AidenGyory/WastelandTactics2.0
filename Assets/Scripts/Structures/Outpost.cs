using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outpost : StructureInfo
{
    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.OutpostMaterial[i] != null)
            {
                modelMaterials[i].material = owner.settings.OutpostMaterial[i];
            }
            else
                modelMaterials[i].material = owner.settings.baseMaterial;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Research : StructureInfo
{
    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.ResearchMaterial[i] != null)
            {
                modelMaterials[i].material = owner.settings.ResearchMaterial[i];
            }
            else
                modelMaterials[i].material = owner.settings.baseMaterial;
        }
    }
}

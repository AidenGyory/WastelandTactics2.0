using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiTankUnit : UnitInfo
{
    public void UpdateMaterials()
    {


        for (int i = 0; i < models.Length; i++)
        {
            if (owner.settings.AntitankUnitMaterial != null)
            {
                Renderer[] _modelRenderers = GetComponentsInChildren<Renderer>();

                foreach (Renderer _renderer in _modelRenderers)
                {
                    _renderer.material = owner.settings.AntitankUnitMaterial;
                }
            }
            else
                models[i].GetComponentInChildren<Renderer>().material = owner.settings.baseMaterial;
        }
    }
}

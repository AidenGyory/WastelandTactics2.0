using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : StructureInfo
{
    public bool active;

    public int metalOutput;

    [SerializeField] GameObject metalRewardPrefab;
    [SerializeField] float distanceOffset;

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
    }

    public void ProduceMetal()
    {
        float _metalScrap = metalOutput; 

        if(owner.MetalProduction)
        {
            _metalScrap += (float)metalOutput * 0.2f; 
        }
        GameObject _AddMetalUI = Instantiate(metalRewardPrefab);
        _AddMetalUI.transform.position = transform.position + Vector3.up * distanceOffset;
        owner.AddMetalScrap(Mathf.RoundToInt(_metalScrap)); 
    }
}

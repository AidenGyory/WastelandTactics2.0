using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Research : StructureInfo
{

    public void UpdateMaterials()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            if (owner.settings.ResearchMaterial != null)
            {
                Renderer[] _modelRenderers = GetComponentsInChildren<Renderer>();

                foreach (Renderer _renderer in _modelRenderers)
                {
                    _renderer.material = owner.settings.ResearchMaterial;
                }
            }
            else
                modelMaterials[i].material = owner.settings.baseMaterial;
        }
    }

    public void AddResearchPoints()
    {
        owner.ResearchPoints++; 
    }

    

    public void OpenResearchMenu()
    {
        if (SelectObjectScript.Instance.canSelect)
        {

            SelectObjectScript.Instance.CameraScreenCanvas.GetComponent<TechTreeMenuUI>().OpenUI();
            TileManager.instance.ClearCanFlipStateOnAllTiles();
            SelectObjectScript.Instance.canSelect = false;

        }
    }

}

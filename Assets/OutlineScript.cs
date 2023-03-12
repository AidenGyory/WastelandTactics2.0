using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineScript : MonoBehaviour
{
    [SerializeField] Transform outlineModel;

    public void TurnOnOutline()
    {
        ChangeLayers(outlineModel, LayerMask.NameToLayer("OutlineLayer")); 
    }

    private void ChangeLayers(Transform parentTransform, LayerMask _layer)
    {
        
        foreach (Transform childTransform in parentTransform)
        {
            // Set the layer to the new layer
            childTransform.gameObject.layer = _layer; 

            // Recursively change the layers of any children
            if (childTransform.childCount > 0)
            {
                ChangeLayers(childTransform, _layer);
            }
        }
    }

    public void TurnOffOutline()
    {
        ChangeLayers(outlineModel, LayerMask.NameToLayer("Default"));
    }
}

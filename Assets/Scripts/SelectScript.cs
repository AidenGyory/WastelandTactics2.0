using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SelectScript : MonoBehaviour
{
    public enum objType
    {
        tile,
    }

    [Header("Object State Info")]
    public objType type;
    [Space]
    public bool isSelected;
    public bool isHighlighted;
    [Space]
    public Renderer[] modelRenderer;

    private Ray _ray;
    private RaycastHit _hit;

    [Header("Colour Overlays")]
    public Color selected;
    public float brightness; 
    private Color[] unselected;

    [Tooltip("Normally set to 0.3f")]
    public float flashSpeed;

    // Start is called before the first frame update
    void Start()
    {
        unselected = new Color[modelRenderer.Length];

        for (int i = 0; i < modelRenderer.Length; i++)
        {
            unselected[i] = modelRenderer[i].material.color; 
        }
    }
    public void Highlight()
    {
        isHighlighted = true;
        for (int i = 0; i < modelRenderer.Length; i++)
        {
            modelRenderer[i].material.DOColor(unselected[i] * brightness, 0.5f);
        }

    }
    public void Unhighlight(Transform _NotThisTransform)
    {
        SelectScript[] obj = FindObjectsOfType<SelectScript>();

        foreach (SelectScript _obj in obj)
        {
            if (_obj.transform != _NotThisTransform)
            {
                if (!isSelected)
                {
                    _obj.isHighlighted = false;
                    _obj.UnhighlightModel(); 
                }
            }
        }
        
    }

    public void UnhighlightModel()
    {
        for (int i = 0; i < modelRenderer.Length; i++)
        {
            modelRenderer[i].material.DOColor(unselected[i], 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //public void SelectObject()
    //{
    //    isSelected = true;

    //    ObjectSelectedFlashUp();


    //    if (type == objType.tile)
    //    {
    //        if (GetComponent<RandomTile>() != null)
    //        {
    //            GetComponent<RandomTile>().FlipTile();
    //        }
    //        else
    //        {
    //            //Select Object 
    //            SelectedObjectDisplayManager.Instance.selectedObject = this.gameObject;
    //            SelectedObjectDisplayManager.Instance.TurnOnTileDisplay();

    //            SelectedObjectActionManager.Instance.UpdateButtons();
    //        }




    //    }

    //    if (type == objType.building)
    //    {
    //        //Move camera to Building View
    //        Camera.main.GetComponent<CameraController>().SetOffset(1);

    //        //Select Object
    //        SelectedObjectDisplayManager.Instance.selectedObject = this.gameObject;
    //        SelectedObjectDisplayManager.Instance.TurnOnBuildingDisplay();

    //        SelectedObjectActionManager.Instance.UpdateButtons();


    //    }
    //}
    //public void Deselect(Transform _NotThisTransform)
    //{
    //    Camera.main.GetComponent<CameraController>().SetOffset(0);

    //    SelectScript[] obj = FindObjectsOfType<SelectScript>();

    //    foreach (SelectScript _obj in obj)
    //    {
    //        if (_obj.transform != _NotThisTransform)
    //        {
    //            if (!isSelected)
    //            {
    //                _obj.isSelected = false;
    //            }
    //        }
    //    }
    //}
    

    //public void ObjectSelectedFlashUp()
    //{
    //    if (isSelected)
    //    {
    //        foreach (Renderer _renderer in modelRenderer)
    //        {
    //            _renderer.material.DOColor(selected, flashSpeed);
    //        }

    //        Invoke("ObjectSelectedFlashBack", flashSpeed);
    //    }

    //}
    //public void ObjectSelectedFlashBack()
    //{
    //    if (isSelected)
    //    {
    //        for (int i = 0; i < modelRenderer.Length; i++)
    //        {
    //            modelRenderer[i].material.DOColor(selected, flashSpeed);
    //        }

    //        Invoke("ObjectSelectedFlashUp", flashSpeed);
    //    }

    //}


}

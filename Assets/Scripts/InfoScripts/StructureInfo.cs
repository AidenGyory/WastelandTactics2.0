using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class StructureInfo : MonoBehaviour
{
    public PlayerInfo owner;
    [Space]
    public string StructureName;
    public Sprite StructureImage;
    [Header("Structure Components")]
    public int maxHealth;
    public int currentHealth;
    public int sightRangeInTiles;
    public int UpgradeLevel; 
    

    [Header("Model Info")]
    public Renderer[] modelMaterials;
    public List<Color> originalColour;

    [SerializeField] UnityEvent UpdatePlayer;
    [SerializeField] UnityEvent runTurnStart;

    public TileInfo occupiedTile; 
    

    public void UpdatePlayerDetails()
    {
        UpdatePlayer.Invoke();

        for (int i = 0; i < modelMaterials.Length; i++)
        {
            originalColour.Add(modelMaterials[i].material.color);
        }
    }

    public void StartTurn()
    {
        runTurnStart.Invoke(); 
    }

    public void SelectStructure()
    {
        foreach (Renderer _model in modelMaterials)
        {
            DOTween.Kill(_model);
            _model.material.DOColor(_model.material.color * TileManager.instance.brightness, TileManager.instance.flashSpeed).SetLoops(-1, LoopType.Yoyo);
        }
        
    }

    public void FocusOnTarget()
    {
        SelectObjectScript.Instance.moveScript.SetDestination(transform.position);
        
        SelectObjectScript.Instance.camScript.SetCameraMode(CameraController.CameraMode.Focused);
    }

    public void UnselectStructure()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            DOTween.Kill(modelMaterials[i].material);
            modelMaterials[i].material.DOColor(originalColour[i], 0.3f);
        }
    }

    public void HighlightStructure()
    {
        foreach (Renderer _model in modelMaterials)
        {

            Color _highlight = _model.material.color * TileManager.instance.brightness;
            DOTween.Kill(_model);
            _model.material.DOColor(_highlight, 0.3f);
        }
    }

    public void unhighlightStructure()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            DOTween.Kill(modelMaterials[i].material);
            modelMaterials[i].material.DOColor(originalColour[i], 0.3f);
        }
    }
}

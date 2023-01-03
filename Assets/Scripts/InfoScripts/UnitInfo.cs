using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UnitInfo : MonoBehaviour
{
    public PlayerInfo owner;
    [Header("Unit Components")]
    public int maxHealth;
    public int currentHealth;
    public int maxMovementTiles;
    public int currentMovementTiles;
    public int baseDamage;
    public int baseDefence; 
    
    [Header("Model Info")]
    public Renderer[] modelMaterials;
    public List<Color> originalColour;

    [SerializeField] UnityEvent UpdatePlayer;


    public void UpdatePlayerDetails()
    {
        UpdatePlayer.Invoke();

        for (int i = 0; i < modelMaterials.Length; i++)
        {
            originalColour.Add(modelMaterials[i].material.color);
        }
    }


    public void SelectUnit()
    {
        foreach (Renderer _model in modelMaterials)
        {
            _model.material.DOColor(_model.material.color * TileManager.instance.brightness, TileManager.instance.flashSpeed).SetLoops(-1, LoopType.Yoyo);
        }
        FocusOnTarget();

    }

    public void FocusOnTarget()
    {
        SelectObjectScript.Instance.moveScript.SetDestination(transform.position);

        SelectObjectScript.Instance.camScript.SetCameraMode(CameraController.CameraMode.Focused);
    }

    public void UnselectUnit()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            DOTween.Kill(modelMaterials[i].material);
            modelMaterials[i].material.DOColor(originalColour[i], 0.3f);
        }
    }

    public void HighlightUnit()
    {
        foreach (Renderer _model in modelMaterials)
        {

            Color _highlight = _model.material.color * TileManager.instance.brightness;
            DOTween.Kill(_model);
            _model.material.DOColor(_highlight, 0.3f);
        }
    }

    public void unhighlightUnit()
    {
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            DOTween.Kill(modelMaterials[i].material);
            modelMaterials[i].material.DOColor(originalColour[i], 0.3f);
        }
    }
}

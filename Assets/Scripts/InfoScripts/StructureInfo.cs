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
    public int powerCost; 
    

    [Header("Model Info")]
    public Renderer[] modelMaterials;
    public List<Color> originalColour;

    [SerializeField] UnityEvent UpdatePlayer;
    [SerializeField] UnityEvent runTurnStart;
    [SerializeField] UnityEvent PlayAction;
    [SerializeField] UnityEvent onDeath; 

    public TileInfo occupiedTile;
    [SerializeField] int BorderRangeinTiles;

    public void UpdatePlayerDetails()
    {
        UpdatePlayer.Invoke();

        for (int i = 0; i < modelMaterials.Length; i++)
        {
            originalColour.Add(modelMaterials[i].material.color);
        }
    }

    public void RefreshStructure()
    {
        runTurnStart.Invoke(); 
    }

    public void CheckAction()
    {
        if (PlayAction != null)
        {
            PlayAction.Invoke();
        }
    }

    public void SelectStructure()
    {
        foreach (Renderer _model in modelMaterials)
        {
            DOTween.Kill(_model);
            _model.material.DOColor(_model.material.color * TileManager.instance.brightness, TileManager.instance.flashSpeed).SetLoops(-1, LoopType.Yoyo);
        }
        
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

    public void Die()
    {
        occupiedTile.isOccupied = false;

        if (onDeath != null)
        {
            onDeath.Invoke();
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void UpdateBorder()
    {

        //create a list of tiles 
        List<TileInfo> _tileList = TileManager.instance.SetTileList(occupiedTile.transform.position, BorderRangeinTiles);

        //check if there are tiles in the list 
        if (_tileList.Count < 1) { return; }

        //iterate through the list of tiles 
        foreach (TileInfo _tile in _tileList)
        {
            //check if the tile is already flipped 
            if (_tile.state == TileInfo.TileState.IsFlipped && _tile.BorderOwner == null)
            {
                //set the owner of the tile to match the owner of the building.  
                _tile.BorderOwner = owner;
                //TileManager.instance.UpdateBorders(); 
            }

        }
    }

    public void UpdateTileOwners()
    {

        //create a list of tiles 
        List<TileInfo> _tileList = TileManager.instance.SetTileList(occupiedTile.transform.position, sightRangeInTiles);

        //check if there are tiles in the list 
        if (_tileList.Count < 1) { return; }

        //iterate through the list of tiles 
        foreach (TileInfo _tile in _tileList)
        {
            //check if the tile is already flipped 
            if (_tile.state == TileInfo.TileState.IsFlipped)
            {
                _tile.Owner = owner; 
            }

        }
    }

    public void SetOccupiedTile()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.2f, TileManager.instance.isTiles);

        foreach (Collider hitCollider in hitColliders)
        {
            //Debug.Log("Found object: " + hitCollider.gameObject.name);
            TileInfo _tile = hitCollider.GetComponent<TileInfo>();

            _tile.isOccupied = true;
            _tile.Owner = owner;
            _tile.BorderOwner = owner;

            occupiedTile = _tile;
        }
        UpdateTileOwners(); 
    }
    
}

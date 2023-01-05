using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Movement Components")]
    [SerializeField] float speed;
    public TileInfo occuipedTile;
    public bool canFly; 


    bool canMove;
    public int moveindex;
    public List<TileInfo> _tilePath;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < _tilePath.Count; i++)
        {
            Gizmos.DrawSphere(_tilePath[i].transform.position,0.5f); 
        }
    }

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
            DOTween.Kill(_model);
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

    public void MoveToTile(TileInfo _targetTile)
    {
        Debug.Log("Target Tile is: " + _targetTile); 
        //Make next tile the occupied tile 
        TileInfo _nextTile = occuipedTile; 

        //Clear Tiles 
        _tilePath.Clear(); 

        int failsafe = 0; 

        // if this does not execute 
        while (_nextTile != _targetTile && failsafe < 10)
        {
            failsafe++;
            List<TileInfo> _tiles = TileManager.instance.SetTileList(_nextTile.transform.position, 1);

            //Check if tile in list is the target tile 
            for (int i = 0; i < _tiles.Count; i++)
            {
                if (_tiles[i] == _targetTile)
                {
                    _tilePath.Add(_tiles[i]);
                    _nextTile = _tiles[i];
                    //Debug.Log("TARGET TILE FOUND "); 
                }
            }
            if(_nextTile != _targetTile)
            {
                _tilePath.Add(TileManager.instance.GetClosestTile(_nextTile.transform, _targetTile.transform, 1, false));
                int lastIndex = _tilePath.Count;
                _nextTile = _tilePath[lastIndex - 1];
            }

             
        }

        if(failsafe > 10)
        {
            Debug.Log("PATH FAILED: " + occuipedTile + " to: " + _targetTile);
            for (int i = 0; i < _tilePath.Count; i++)
            {
                Debug.Log("tile path: " + _tilePath[i]);
            }
        }
        else
        {
            SelectObjectScript.Instance.canSelect = false;
            GetComponent<SelectScript>().DeselectObject();
            currentMovementTiles -= _tilePath.Count;
            canMove = true;

            occuipedTile.isOccupied = false;
            occuipedTile = _targetTile;
            occuipedTile.isOccupied = true;
        }
        

    }

    private void Update()
    {
        if (canMove && moveindex < _tilePath.Count)
        {

            if (Vector3.Distance(transform.position, _tilePath[moveindex].transform.position) > 0.1f)
            {
                transform.DODynamicLookAt(_tilePath[moveindex].transform.position,0.2f); 
                transform.position += transform.forward * (Time.deltaTime * speed);
            }
            else
            {
                moveindex++;
            }
        }
        else if(canMove)
        {
            SelectObjectScript.Instance.canSelect = true;
            SelectObjectScript.Instance.SetModeToSelect();
            TileManager.instance.ClearMoveableStateOnAllTiles();
            moveindex = 0;
            occuipedTile = _tilePath[_tilePath.Count - 1];
            occuipedTile.isOccupied = true; 
            _tilePath.Clear();

            canMove = false;
            if(currentMovementTiles > 0)
            {
                GetComponent<SelectScript>().SelectObject();
            }
            
        }
    }


}

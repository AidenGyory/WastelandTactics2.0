using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitInfo : MonoBehaviour
{
    
    public PlayerInfo owner;
    [Space]
    public string unitName;
    public Sprite unitImage;
    [Header("Unit Components")]
    public bool canFly;
    public int maxMovementTiles;
    public int currentMovementTiles;
    public int powerCost; 

    [Header("Model Info")]
    public GameObject[] models;
    public Color originalColour;

    [SerializeField] UnityEvent UpdatePlayer;
    [SerializeField] UnityEvent PlayAction;
    [SerializeField] UnityEvent OnTurnStart; 

    [Header("Movement Components")]
    [SerializeField] float speed;
    
    public TileInfo occuipedTile;
    public LayerMask isUnits;

    bool canMove;
    public int moveindex;
    public List<TileInfo> _tilePath;
    public List<Material> _ModelMaterials;

    private HealthScript _health;
    private UnitAttackScript _attackScript; 

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < _tilePath.Count; i++)
        {
            Gizmos.DrawSphere(_tilePath[i].transform.position,0.5f); 
        }
    }

    private void Start()
    {
        _ModelMaterials.Clear();
        GameManager.Instance.currentPlayerTurn.UpdatePlayerPowerSupply();
        _health = GetComponent<HealthScript>();
        _attackScript = GetComponent<UnitAttackScript>(); 
    }
    public void UpdatePlayerDetails()
    {
        UpdatePlayer.Invoke();

        Renderer[] _modelAmount = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < _modelAmount.Length; i++)
        {
            _ModelMaterials.Add(_modelAmount[i].material); 
        }

        originalColour = _ModelMaterials[0].color;
    }


    public void SelectUnit()
    {
        foreach (Material _material in _ModelMaterials)
        {
            DOTween.Kill(_material);
            _material.DOColor(_material.color * TileManager.instance.brightness, TileManager.instance.flashSpeed).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void UnselectUnit()
    {

        foreach (Material _material in _ModelMaterials)
        {
            DOTween.Kill(_material);
            _material.DOColor(originalColour, 0.3f);
        }
    }

    public void HighlightUnit()
    {

        foreach (Material _material in _ModelMaterials)
        {

            Color _highlight = _material.color * TileManager.instance.brightness;
            DOTween.Kill(_material);
            _material.DOColor(_highlight, 0.3f);
        }
    }

    public void unhighlightUnit()
    {

        foreach (Material _material in _ModelMaterials)
        {
            DOTween.Kill(_material);
            _material.DOColor(originalColour, 0.3f);
        }
    }

    public void MoveToTile(TileInfo _targetTile)
    {
        //Clear Tiles 
        _tilePath = PathfindingManager.Instance.GetPath(occuipedTile, _targetTile, canFly); 

        if(_tilePath != null)
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

            if (Vector3.Distance(transform.position, _tilePath[moveindex].transform.position) > 0.05f)
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
            //SelectObjectScript.Instance.SetModeToSelect();
            TileManager.instance.ClearMoveableStateOnAllTiles();
            moveindex = 0;
            occuipedTile = _tilePath[_tilePath.Count - 1];
            occuipedTile.isOccupied = true; 
            _tilePath.Clear();

            canMove = false;

            
        }
    }

    public void CheckMovement()
    {
        //Debug.Log("Check Unit Movement"); 
        if (owner == GameManager.Instance.currentPlayerTurn)
        {
            //Set Selection Mode (Move unit Mode) 
            SelectObjectScript.Instance.mode = SelectObjectScript.PointerMode.UnitMode;

            //Create a list of all "moveable" tiles. 
            List<TileInfo> _moveableTiles = TileManager.instance.SetTileList(transform.position, currentMovementTiles);

            // set tiles to moveable from list ignoring terrain
            TileManager.instance.SetTilesAsMoveable(occuipedTile,_moveableTiles, canFly);
        }
    }

    public void StartTurn()
    {
        currentMovementTiles = maxMovementTiles;
        if(OnTurnStart != null)
        {
            OnTurnStart.Invoke();
        }
         

    }

    public void CheckAction()
    {
        if(PlayAction != null)
        { 
            PlayAction.Invoke();
        }
    }

    public void Die()
    {
        occuipedTile.isOccupied = false; 
        Destroy(gameObject); 
    }

}

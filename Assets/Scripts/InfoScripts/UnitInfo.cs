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
    public int maxHealth;
    public int currentHealth;
    public bool canFly;
    public int maxMovementTiles;
    public int currentMovementTiles;
    public int baseDamage;
    public int attackRange; 
    public int prestigeLevel;

    [Header("Model Info")]
    public GameObject[] models;
    public Color originalColour;

    [SerializeField] UnityEvent UpdatePlayer;

    [Header("Movement Components")]
    [SerializeField] float speed;
    
    public TileInfo occuipedTile;
    public LayerMask isUnits;

    [Header("Attack Components")]
    public bool canAttack;
    public bool isTarget; 
    public GameObject AttackCanvas;
    public GameObject HealthCanvas; 
    public Image healthBarUI; 

    bool canMove;
    public int moveindex;
    public List<TileInfo> _tilePath;
    public List<Material> _ModelMaterials;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < _tilePath.Count; i++)
        {
            Gizmos.DrawSphere(_tilePath[i].transform.position,0.5f); 
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); 
    }

    private void Start()
    {
        _ModelMaterials.Clear(); 
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
        FocusOnTarget();

    }

    public void FocusOnTarget()
    {
        SelectObjectScript.Instance.moveScript.SetDestination(transform.position);

        SelectObjectScript.Instance.camScript.SetCameraMode(CameraController.CameraMode.Focused);
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
        //Debug.Log("Target Tile is: " + _targetTile); 
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
        if(currentHealth < maxHealth)
        {
            HealthCanvas.SetActive(true);
            float _targetHealth = (float)currentHealth / (float)maxHealth;
            healthBarUI.fillAmount = Mathf.Lerp(healthBarUI.fillAmount, _targetHealth, Time.deltaTime * 8); 
        }
    }

    public void CheckMovement()
    {
        if (owner == GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn])
        {
            //Set Selection Mode (Move unit Mode) 
            SelectObjectScript.Instance.mode = SelectObjectScript.PointerMode.MoveMode;

            //Create a list of all "moveable" tiles. 
            List<TileInfo> _moveableTiles = TileManager.instance.SetTileList(transform.position, currentMovementTiles);

            // set tiles to moveable from list ignoring terrain
            TileManager.instance.SetTilesAsMoveable(_moveableTiles, canFly);
        }
    }

    public void CheckAttack()
    {
        if(owner == GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn] && canAttack)
        {
            SelectObjectScript.Instance.mode = SelectObjectScript.PointerMode.AttackMode;

            Collider[] _units = Physics.OverlapSphere(transform.position, attackRange, isUnits);

            for (int i = 0; i < _units.Length; i++)
            {
                if (_units[i].GetComponent<UnitInfo>().owner != GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn])
                {
                    Debug.Log("Can Attack" + _units[i].GetComponent<UnitInfo>().unitName);
                    _units[i].GetComponent<UnitInfo>().AttackCanvas.SetActive(true);
                    _units[i].GetComponent<UnitInfo>().isTarget = true; 
                }
            }
        }
        else
        {
            Debug.Log("Can't attack"); 
        }
    }

}

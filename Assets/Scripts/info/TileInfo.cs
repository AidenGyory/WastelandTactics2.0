using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SelectScript))]
public class TileInfo : MonoBehaviour
{
    public enum TileState
    {
        cannotFlip,
        canFlip,
        isFlipped,
    }

    public enum TileType
    {
        Mountain,
        Quicksand,
        Desert1,
        Desert2,
        Quarry,
        Mine,
        Rockpile
    }

    public enum IsWalkable
    {
        canWalk,
        cantWalk,
        unset
    }

    [Header("Tile Info")]
    public string tileName;
    public string tileDescription;
    public TileType type;
    public int movementCost;
    public bool isResourceTile;
    public Sprite resourceIcon;
    public int resourceAmountLeft;

    [Header("Tile Components")]
    public TileState state;
    public bool clearThisTile;
    public IsWalkable canWalk; 
    public Renderer[] modelRenderer;

    
    private Color[] unselected;
    

    [Header("Tile Filters")]
    [SerializeField] LayerMask isSpawn;
    [SerializeField] LayerMask isStructure;
    [SerializeField] LayerMask isUnit;

    [Header("Objects Attached to Tile")]
    public GameObject structureOnTile;
    public GameObject unitOnTile; 

    //PRIVATE FUNCTIONS
    void Start()
    {
        if(!clearThisTile)
        {
            Collider[] _spawnTiles = Physics.OverlapSphere(transform.position, 0.5f, isSpawn);

            if (_spawnTiles.Length > 0)
            {
                ChangeTileToClearForBaseSpawn();
            }

            if (state != TileState.isFlipped)
            {
                transform.Rotate(180f, 0f, 0f); 
            }
        }

        unselected = new Color[modelRenderer.Length];

        for (int i = 0; i < modelRenderer.Length; i++)
        {
            unselected[i] = modelRenderer[i].material.color;
        }


    }
    
    void RotateTileBasedOnTileState()
    {
        if (state != TileState.isFlipped)
        {
            transform.DORotate(new Vector3(180f, 0f, 0f), 0.3f);
            transform.DOJump(transform.position, 0.2f, 1, 0.3f).OnComplete(GetComponent<SelectScript>().ClearSelectInfo);
        }
        else
        {
            transform.DORotate(Vector3.zero, 0.3f);
            transform.DOJump(transform.position, 0.2f, 1, 0.3f).OnComplete(GetComponent<SelectScript>().ClearSelectInfo);
        }
    }

    void ChangeTileToClearForBaseSpawn()
    {
        GameObject _tile = Instantiate(StructuresManager.Instance.clearTile[Random.Range(0,StructuresManager.Instance.clearTile.Length)]);
        _tile.transform.position = transform.position;
        _tile.name = gameObject.name;
        _tile.GetComponent<TileInfo>().state = TileState.isFlipped; 
        _tile.GetComponent<TileInfo>().clearThisTile = true;
        _tile.transform.SetParent(transform.parent);
        Destroy(gameObject); 
    }

    // PUBLIC FUNCTIONS 
    public void SetTileState(TileState _state)
    {
        state = _state;

        switch (state)
        {
            case TileState.cannotFlip:
                //Execute if State is set to cannot Flip
                
                break;
            case TileState.canFlip:
                //Execute if State is set to can Flip

                break;
            case TileState.isFlipped:
                //Execute if State is set to isFlipped

                break;
        }
        RotateTileBasedOnTileState(); 
    }
    public void SetTileWalkableStatus(bool _isWalkable)
    {
        if(_isWalkable)
        {
            canWalk = IsWalkable.canWalk;
            //if(type == TileType.Mountain || type == TileType.Quicksand)
            //{
            //    canWalk = IsWalkable.cantWalk; 
            //}
            //else
            //{
            //    canWalk = IsWalkable.canWalk; 
            //}
        }
        else
        {
            canWalk = IsWalkable.unset; 
        } 
    }
    public void AttachStructureToThisTile()
    {
        Collider[] _structures = Physics.OverlapSphere(transform.position, 0.1f, isStructure);

        foreach (Collider _structure in _structures)
        {
            Debug.Log("hit" + _structure.gameObject.name);
            structureOnTile = _structure.gameObject;
        }
    }
    public void AttachUnitToThisTile()
    {
        Collider[] _units = Physics.OverlapSphere(transform.position, 0.1f, isUnit);

        foreach (Collider _unit in _units)
        {
            Debug.Log("hit" + _unit.gameObject.name);
            structureOnTile = _unit.gameObject;
        }
    }
    public void ChangeMaterialAndScale(SelectScript.SelectState currentSelectState, float _timer)
    {
        if(canWalk == IsWalkable.unset)
        {
            switch (currentSelectState)
            {
                case SelectScript.SelectState.Unselected:
                    for (int i = 0; i < modelRenderer.Length; i++)
                    {
                        DOTween.Kill(modelRenderer[i].material);
                        modelRenderer[i].material.DOColor(unselected[i], _timer);
                        transform.DOScaleY(1f, 0.5f);
                    }
                    break;
                case SelectScript.SelectState.Highlighted:
                    for (int i = 0; i < modelRenderer.Length; i++)
                    {
                        DOTween.Kill(modelRenderer[i].material);
                        modelRenderer[i].material.DOColor(unselected[i] * TileManager.Instance.brightness, 0.5f);
                        transform.DOScaleY(1.5f, 0.5f);
                    }
                    break;
                case SelectScript.SelectState.Selected:
                    for (int i = 0; i < modelRenderer.Length; i++)
                    {
                        DOTween.Kill(modelRenderer[i].material);
                        modelRenderer[i].material.DOColor(unselected[i] * TileManager.Instance.selected * TileManager.Instance.brightness, 0.5f).SetLoops(-1, LoopType.Yoyo);
                        transform.DOScaleY(1.8f, 0.5f);
                    }
                    break;
            }
        }
        else
        {
            if (currentSelectState == SelectScript.SelectState.Highlighted)
            {
                for (int i = 0; i < modelRenderer.Length; i++)
                {
                    DOTween.Kill(modelRenderer[i].material);
                    modelRenderer[i].material.DOColor(unselected[i] * TileManager.Instance.brightness, 0.5f);
                }
            }
            if (currentSelectState == SelectScript.SelectState.Unselected)
            {
                for (int i = 0; i < modelRenderer.Length; i++)
                {
                    ChangeToWalkableMaterial(); 
                }
            }
        }
        
    }
    public void ChangeToWalkableMaterial()
    {
        if(canWalk == IsWalkable.canWalk)
        {
            for (int i = 0; i < modelRenderer.Length; i++)
            {
                DOTween.Kill(modelRenderer[i].material);
                modelRenderer[i].material.DOColor(unselected[i] * TileManager.Instance.walkable * TileManager.Instance.brightness, 0.5f);
                transform.DOScaleY(1.5f, 0.5f);
            }
        }
        if(canWalk == IsWalkable.cantWalk)
        {
            for (int i = 0; i < modelRenderer.Length; i++)
            {
                DOTween.Kill(modelRenderer[i].material);
                modelRenderer[i].material.DOColor(unselected[i] * TileManager.Instance.unwalkable * TileManager.Instance.brightness, 0.5f);
                transform.DOScaleY(1.5f, 0.5f);
            }
        }
        
    }
    public void ChangeToPlaceableMaterial()
    {
        if (canWalk == IsWalkable.canWalk)
        {
            for (int i = 0; i < modelRenderer.Length; i++)
            {
                DOTween.Kill(modelRenderer[i].material);
                modelRenderer[i].material.DOColor(unselected[i] * TileManager.Instance.placeable * TileManager.Instance.brightness, 0.5f);
                transform.DOScaleY(1.5f, 0.5f);
            }
        }
        if (canWalk == IsWalkable.cantWalk)
        {
            for (int i = 0; i < modelRenderer.Length; i++)
            {
                DOTween.Kill(modelRenderer[i].material);
                modelRenderer[i].material.DOColor(unselected[i] * TileManager.Instance.unwalkable * TileManager.Instance.brightness, 0.5f);
                transform.DOScaleY(1.5f, 0.5f);
            }
        }

    }
}

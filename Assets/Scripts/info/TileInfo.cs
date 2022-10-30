using UnityEngine;
using DG.Tweening;
using static SelectScript;

public class TileInfo : MonoBehaviour
{
    public enum TileState
    {
        cannotFlip,
        canFlip,
        isFlipped,
    }

    [Header("Tile Info")]
    public string tileName;
    public string tileDescription;
    public bool isResourceTile;
    public Sprite resourceIcon;
    public int resourceAmountLeft;


    [Header("Tile Components")]
    public TileState state;
    public bool isClear; 
    [SerializeField] LayerMask isSpawn;
    [SerializeField] LayerMask isStructure;
    [SerializeField] LayerMask isUnit; 
 
    public GameObject structureOnTile;
    public GameObject unitOnTile; 

    void Start()
    {
        if(!isClear)
        {
            Collider[] _spawnTiles = Physics.OverlapSphere(transform.position, 0.5f, isSpawn);

            if (_spawnTiles.Length > 0)
            {
                ClearTile();
            }

            StateSwitch();
        }
    }
    public void AttachStructure()
    {
        Collider[] _structures = Physics.OverlapSphere(transform.position, 0.1f, isStructure);

        foreach (Collider _structure in _structures)
        {
            Debug.Log("hit" + _structure.gameObject.name);
            structureOnTile = _structure.gameObject;
        }
    }

    public void AttachUnit()
    {
        Collider[] _units = Physics.OverlapSphere(transform.position, 0.1f, isUnit);

        foreach (Collider _unit in _units)
        {
            Debug.Log("hit" + _unit.gameObject.name);
            structureOnTile = _unit.gameObject;
        }
    }
    public void StateSwitch()
    {
        switch (state)
        {
            case TileState.cannotFlip:
                transform.eulerAngles = new Vector3(180f, 0f, 0f);
                break;
            case TileState.canFlip:
                transform.eulerAngles = new Vector3(180f, 0f, 0f);
                break;
            case TileState.isFlipped:

                break;
        }
    }

    public void ClearTile()
    {
        GameObject _tile = Instantiate(Structures.Instance.clearTile[Random.Range(0,Structures.Instance.clearTile.Length)]);
        _tile.transform.position = transform.position;
        _tile.name = gameObject.name;
        _tile.GetComponent<TileInfo>().state = TileState.isFlipped; 
        _tile.GetComponent<TileInfo>().isClear = true;
        _tile.transform.SetParent(transform.parent);
        Destroy(gameObject); 
    }

    public void CheckState()
    {
        switch (state)
        {
            case TileState.cannotFlip:


                break;
            case TileState.canFlip:

                state = TileState.isFlipped;

                transform.DORotate(Vector3.zero, 0.3f);
                transform.DOJump(transform.position, 0.2f, 1, 0.3f).OnComplete(ClearSelectInfo);

                break;
            case TileState.isFlipped:

                transform.DOScaleY(2f, 0.5f);
                
                break;
        } 
    }

    public void SetTileState(TileState _state)
    {
        state = _state; 

        switch(state)
        {
            case TileState.cannotFlip:


                break;
            case TileState.canFlip:

                ChangeMaterial(GetComponent<SelectScript>()); 

                break;
            case TileState.isFlipped:


                break; 
        }
    }

    void ClearSelectInfo()
    {
        GetComponent<SelectScript>().Deselect();
        GetComponent<SelectScript>().Unhighlight();
    }

    public void ChangeMaterial(SelectScript _tile)
    {
        if (_tile.isHighlighted || _tile.isSelected) { return; } //don't start highlight if already highlighted
        _tile.isHighlighted = true;
        for (int i = 0; i < _tile.modelRenderer.Length; i++)
        {
            DOTween.Kill(_tile.modelRenderer[i].material);
            _tile.modelRenderer[i].material.DOColor(Color.white, 0.5f);
            if (_tile.type == objType.tile)
            {
                transform.DOScaleY(1.5f, 0.5f);
            }
        }
    }
}

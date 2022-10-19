using UnityEngine;


public class TileInfo : MonoBehaviour
{
    public enum TileState
    {
        cannotFlip,
        canFlip,
        isFlipped,
    }

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

    

}

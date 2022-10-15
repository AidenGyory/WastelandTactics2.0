using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

    

    // Start is called before the first frame update
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

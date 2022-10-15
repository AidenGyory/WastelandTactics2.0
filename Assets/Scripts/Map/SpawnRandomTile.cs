using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class TilePrefabs
{
    public GameObject tile;
    public int weight;
}
public class SpawnRandomTile : MonoBehaviour
{
    public TilePrefabs[] tiles;
    [SerializeField] LayerMask mapShaperLayer; 



    // Start is called before the first frame update
    void Start()
    {
        Collider[] _shape = Physics.OverlapSphere(transform.position, 1f, mapShaperLayer);

        if(_shape.Length < 1 || _shape == null)
        {
            Destroy(gameObject);
            return;
        }
        else
        RandomNumber(); 

    }
    void RandomNumber()
    {
        int _total = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            _total += tiles[i].weight;
        }

        int _rand = Random.Range(0, _total);
        FindTile(_rand, 0);
    }
    // Update is called once per frame
    void FindTile(int _num, int _index)
    {
        _num -= tiles[_index].weight;

        if(_num <= 0)
        {
            SpawnTile(_index);
        }
        else
        {
            FindTile(_num, _index + 1);
        }
    }
    void SpawnTile(int _index)
    {
        GameObject _tile = Instantiate(tiles[_index].tile);
        _tile.transform.position = transform.position;
        _tile.name = gameObject.name; 
        _tile.transform.SetParent(transform.parent);

        Destroy(gameObject); 

    }
}

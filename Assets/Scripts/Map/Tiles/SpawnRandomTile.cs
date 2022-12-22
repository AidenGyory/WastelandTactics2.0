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
    [SerializeField] LayerMask spawnTiles;

    void Start()
    {
        Collider[] _shape = Physics.OverlapSphere(transform.position, 1f, mapShaperLayer);

        if (_shape.Length < 1 || _shape == null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            var _spawn = Physics.OverlapSphere(transform.position, 0.2f, spawnTiles);

            if (_spawn.Length > 0)
            {
                SpawnTile(0, true);
            }
            else
            {
                var totalWeight = 0;
                for (int i = 0; i < tiles.Length; i++)
                {
                    totalWeight += tiles[i].weight;
                }

                var randomNumber = Random.Range(0, totalWeight +1);
                int index = 0;
                for (int i = 0; i < tiles.Length; i++)
                {
                    randomNumber -= tiles[i].weight;
                    if (randomNumber <= 0)
                    {
                        index = i;
                        break;
                    }
                }
                SpawnTile(index, false);
            }
        }
    }

    void SpawnTile(int index, bool flipped)
    { 
        var _tile = Instantiate(tiles[index].tile);
        _tile.transform.position = transform.position;
        _tile.name = gameObject.name;
        _tile.transform.SetParent(transform.parent);
        if(flipped)
        {
            _tile.GetComponent<TileInfo>().state = TileInfo.TileState.IsFlipped; 
        }

        

        Destroy(gameObject);
    }
}

using Unity.VisualScripting;
using UnityEngine;


// Create an Array with multiple variables per element. 
[System.Serializable]
public class TilePrefabs
{
    //Tile type prefab
    public GameObject tile;
    //Weight for randomness
    public int weight;
}
public class SpawnRandomTile : MonoBehaviour
{
    public LayerMask mapShaperLayer;
    public LayerMask spawnPointLayer;
    //Array of Tile types and weights
    [SerializeField] TilePrefabs[] tiles;

    bool clearTile; 

    private void Start()
    {
        CheckTileType();      
    }

    void CheckTileType()
    {
        // Check for overlap with objects on MapShaper layer
        Collider[] toDestroy = Physics.OverlapSphere(transform.position, 1, mapShaperLayer);

        // If there's at least one overlapping object
        if (toDestroy.Length < 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Collider[] spawnpoint = Physics.OverlapSphere(transform.position, 0.5f, spawnPointLayer);

            // If there's at least one overlapping object
            if (spawnpoint.Length > 0)
            {
                clearTile = true;
            }
            ChooseTileType();
        }
        
    }


    void ChooseTileType()
    {
        int totalWeight = 0;

        for (int i = 0; i < tiles.Length; i++)
        {
            totalWeight += tiles[i].weight;
        }

        int randomNumber = Random.Range(0, totalWeight + 1);
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
        //Create Tile 
        CreateTilefromTypeList(index);
    }

    void CreateTilefromTypeList(int index)
    { 
        GameObject _tile = Instantiate(tiles[index].tile);
        _tile.transform.position = transform.position;
        _tile.name = gameObject.name;
        _tile.transform.SetParent(transform.parent);

        if (clearTile)
        {
            _tile.GetComponent<TileInfo>().state = TileInfo.TileState.IsFlipped;
            _tile.GetComponent<TileInfo>().Checkable = true; 
            _tile.GetComponent<TileInfo>().SetToClearTile(); 
        }


        Destroy(gameObject);
    }
}

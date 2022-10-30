using UnityEngine;

public class PlayerSpawnLocations : MonoBehaviour
{
    [SerializeField] GameObject[] playerSpawns;
     
    // Start is called before the first frame update
    public void TurnOnSpawnLocations(int index)
    {
        foreach (GameObject spawns in playerSpawns)
        {
            spawns.SetActive(false);
        }

        playerSpawns[index].SetActive(true);

        for (int i = 0; i <= (int)GameManager.Instance.amountOfPlayers +1; i++)
        { 
            Vector3 _pos = playerSpawns[index].transform.GetChild(i).position;
            GameObject _HQ = Instantiate(Structures.Instance.HQ); 
            _HQ.transform.position = _pos;
            
            StructureInfo _info = _HQ.GetComponent<StructureInfo>();
            _info.factionOwner = PlayerManager.Instance.players[i];
            _info.UpdatePlayerDetails(); 
        }
    }
}

using UnityEngine;

public class PlayerSpawnLocations : MonoBehaviour
{
    [SerializeField] GameObject[] playerSpawns;

    public void TurnOnSpawnLocations(int index)
    {
        foreach (GameObject spawns in playerSpawns)
        {
            spawns.SetActive(false);
        }

        playerSpawns[index].SetActive(true);

        index += 2; 
        Debug.Log("Set Player Spawns for " + index + " Players."); 
    }
}

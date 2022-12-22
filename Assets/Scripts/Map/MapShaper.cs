using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapShaper : MonoBehaviour
{

    [SerializeField] GameObject[] shapes;
    int index;
    int playerSpawns; 

    // Start is called before the first frame update
    void Start()
    {
        index = (int)GameManager.Instance.mapType;
        playerSpawns = (int)GameManager.Instance.amountOfPlayers - 2; 

        foreach(GameObject shape in shapes)
        {
            shape.SetActive(false); 
        }

        shapes[(int)index].SetActive(true);
        shapes[(int)index].GetComponent<PlayerSpawnLocations>().TurnOnSpawnLocations((int)playerSpawns);

        Invoke("TurnOff", 1f);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false); 
    }
}

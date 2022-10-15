using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum MapIndex
    {
        Donut = 0,
        Oval = 1,
        Rectangle = 2,
        Square = 3,
    }

    public enum PlayerSpawns
    {
        twoPlayer = 0,
        threePlayer = 1,
        fourPlayer = 2,
    }

    
    [Header("Game Settings")]
    public MapIndex mapType;
    public PlayerSpawns amountOfPlayers;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

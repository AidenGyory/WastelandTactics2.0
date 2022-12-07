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

    public enum PlayerTurn
    {
        wait = 0,
        player1 = 1,
        player2 = 2, 
        player3 = 3, 
        player4 = 4,
    }

    
    [Header("Game Settings")]
    public MapIndex mapType;
    public PlayerSpawns amountOfPlayers;

    int players;
    PlayerTurn turn; 


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
    private void Start()
    {
        players = (int)amountOfPlayers + 2;
        turn = PlayerTurn.player1; 
        
    }
    public void SetPlayerTurn(bool local_nextPlayer)
    {
        if(local_nextPlayer)
        {
            if ((int)turn >= players)
            {
                turn = PlayerTurn.player1; 
            }
            else
            {
                turn += 1; 
            }
        }
    }
}

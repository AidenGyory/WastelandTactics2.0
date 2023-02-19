using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

//REMINDER: THIS SCRIPT SHOULD BE CREATED BEFORE ENTERING THE GAME SCENE
public class GameManager : MonoBehaviour
{
    // This variable holds a reference to the singleton instance of the GameManager script.
    public static GameManager Instance;

    // This enum lists the different types of maps that can be selected.
    public enum MapIndex
    {
        Donut = 0,
        Oval = 1,
        Rectangle = 2,
        Square = 3,
    }

    // This enum lists the different options for the number of players that can spawn.
    public enum PlayerSpawns
    {
        zeroPlayers = 0,
        onePlayer = 1,
        twoPlayer = 2,
        threePlayer = 3,
        fourPlayer = 4,
    }

    // This enum lists the different player turns.
    public enum PlayerTurn
    {
        player1,
        player2,
        player3,
        player4,
    }

    // This variable holds the current player turn.
    public PlayerTurn currentPlayerTurn;
    public PlayerInfo currentPlayer; 

    // These variables store settings for encounters.
    [Header("Encounter Settings")]
    public MapIndex mapType;
    public PlayerSpawns amountOfPlayers;
    public PlayerInfo[] playerInfo;
    public bool randomiseFirstPlayer;
    public int turnTimer; 

    // This variable holds a reference to the headquarters game object.
    [Header("Structure Prefabs")]
    public GameObject HQPrefab;

    // This variable stores the number of players.
    [SerializeField] int players;


    // This script sets up a singleton instance of itself that persists across scene changes.
    void Awake()
    {
        // If there is no instance of the script, set this instance as the singleton.
        if (Instance == null)
        {
            // Prevent the instance from being destroyed when a new scene is loaded.
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        // If there is already an instance of the script, destroy this instance.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    //Invoked through the tile manager once the game is ready to be played. 
    public void StartGame()
    {
        // Initialize the number of players.
        players = (int)amountOfPlayers;

        // If there are not enough players to play, log an error and return.
        if (players < 2) { Debug.LogError("NOT ENOUGH PLAYERS TO PLAY!!"); return; }

        // Set the current player turn either randomly or to player 1, depending on the value of randomiseFirstPlayer.
        currentPlayerTurn = randomiseFirstPlayer ? (PlayerTurn)Random.Range(0, players + 1) : PlayerTurn.player1;
        //Initilize the first player
        currentPlayer = playerInfo[(int)currentPlayerTurn];
        InitializePlayerTurn();
        // Allow objects to be selected.
        SelectObjectScript.Instance.canSelect = true;

        
    }

    // This function sets the current player turn to the next player and initiates their turn.
    public void SetNextPlayersTurn()
    {
        // Increment the index of the current player.
        int _currentPlayer = (int)currentPlayerTurn + 1;

        // If the current player is the last player, set the index to 0 (start with the first player again).
        if (_currentPlayer >= players)
        {
            turnTimer += 1; 
            _currentPlayer = 0;
        }

        // Set the current player turn based on the updated index.
        currentPlayerTurn = (PlayerTurn)_currentPlayer;

        // Initiate the turn for the new current player.
        currentPlayer = playerInfo[(int)currentPlayerTurn];
        InitializePlayerTurn();
    }

    // This function checks if the current player has enough exploration points to perform a certain action.
    public bool CheckExplorationPoints()
    {
        // Check if the current player has enough exploration points.
        bool hasEnoughPoints = currentPlayer.ExplorationPointsLeft > 0;

        // If they don't, log a message.
        if (!hasEnoughPoints)
        {
            //Debug.Log("Not Enough Points!!");
        }

        return hasEnoughPoints;
    }

    // This function initialize the turn for the current player by refreshing their stats.
    public void InitializePlayerTurn()
    {
        SelectObjectScript.Instance.SelectPlayer(currentPlayer);
        //Refresh all tile Scans
        foreach (TileInfo _tile in TileManager.instance.allTiles)
        {
            _tile.ShowScanIcon(false); 
        }

        // Refresh the player's exploration points.
        currentPlayer.AddPoints(ResourcesType.ExplorationPoints, currentPlayer.ExplorationPointsMax);

        //Find all flipable tiles for current player
        TileManager.instance.FindPlayerOwnedTilesForFlipCheck(currentPlayer);

        //Replenish all Units
        UnitInfo[] _units = FindObjectsOfType<UnitInfo>();

        foreach (UnitInfo _unit in _units)
        {
            if(_unit.owner == currentPlayer)
            {
                _unit.ReplenishUnit(); 
            }
        }

        //Replenish all Stuctures
        StructureInfo[] _structures = FindObjectsOfType<StructureInfo>();

        foreach(StructureInfo _structure in _structures)
        {
            if(_structure.owner == currentPlayer)
            {
                _structure.StartTurn(); 
            }
        }
        UpdatePowerLevel();
        CheckForMalfunction();

        
    }

    public void UpdatePowerLevel()
    {
        int PowerNeeded = 0; 

        UnitInfo[] _units = FindObjectsOfType<UnitInfo>();
        StructureInfo[] _Structures = FindObjectsOfType<StructureInfo>();

        for (int i = 0; i < _units.Length; i++)
        {
            if (_units[i].owner == currentPlayer)
            {
                PowerNeeded += _units[i].powerCost; 
            }
        }

        for (int i = 0; i < _Structures.Length; i++)
        {
            if (_Structures[i].owner == currentPlayer)
            {
                PowerNeeded += _Structures[i].powerCost;
            }
        }

        currentPlayer.PowerSupplyUsed = PowerNeeded; 
    }

    void CheckForMalfunction()
    {
        if(currentPlayer.PowerSupplyTotal < currentPlayer.PowerSupplyUsed)
        {
            CreateMalfunction(); 
        }
    }

    void CreateMalfunction()
    {
        Debug.Log(currentPlayer.settings.playerName + " is in a power defict!!"); 
    }
}

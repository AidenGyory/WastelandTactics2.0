using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MapProfileTemplateSO mapProfile;

    public static GameManager Instance;

    [Header("Current Turn")]
    public PlayerInfo currentPlayerTurn;
    public int turnTimer;
    [Space]
    [Header("Player Data")]
    public PlayerInfo[] players;
    public GameObject[] HQPrefabs;
    int playerIndex; 

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
        // If there are not enough players to play, log an error and return.
        if (mapProfile.amountOfPlayers < 2) { Debug.LogError("NOT ENOUGH PLAYERS TO PLAY!!"); return; }

        playerIndex = 0; 

        //If randomise first player is checked, then set a random first player
        if(mapProfile.randomiseFirstPlayer)
        {
            playerIndex = Random.Range(0, mapProfile.amountOfPlayers); 
        }

        currentPlayerTurn = players[playerIndex];

        SelectObjectScript.Instance.SetModeToSelect();

        //Start Player Turn
        Invoke("StartTurn", 1f); 
    }
    public void SetNextPlayersTurn()
    {

        playerIndex += 1;

        if (playerIndex > mapProfile.amountOfPlayers - 1)
        {
            playerIndex = 0;
            turnTimer += 1;
        }

        currentPlayerTurn = players[playerIndex];

        StartTurn();
    }
    void StartTurn()
    {
        DOTween.KillAll();


        TileManager.instance.SetBorderTileOwnership();
        //Select current player for zoom back to HQ
        SelectObjectScript.Instance.SelectPlayer(currentPlayerTurn);

        //Remove icons from Scanned Tiles
        TileManager.instance.ResetIconsOnTiles();

        // Refresh the player's exploration points.
        currentPlayerTurn.AddExplorationPoints(currentPlayerTurn.ExplorationPointsMax);

        currentPlayerTurn.CheckForMalfunctions();

        AddEffectsFromRelicsAndMalfunctions();

        //Refresh all Player Objects
        RefreshAllPlayerUnits();

        RefreshAllPlayerStructures();

        //Find all flipable tiles for current player
        TileManager.instance.FindPlayerOwnedTilesForFlipCheck(currentPlayerTurn);
    }
    public void RefreshAllPlayerUnits()
    {
        UnitInfo[] _units = FindObjectsOfType<UnitInfo>();

        foreach (UnitInfo _unit in _units)
        {
            if (_unit.owner == currentPlayerTurn)
            {
                _unit.RefreshUnit();
            }
        }
    }
    public void RefreshAllPlayerStructures()
    {
        StructureInfo[] _structures = FindObjectsOfType<StructureInfo>();

        foreach (StructureInfo _structure in _structures)
        {
            if (_structure.owner == currentPlayerTurn)
            {
                _structure.RefreshStructure();
            }
        }
    }
    public void AddEffectsFromRelicsAndMalfunctions()
    {
        //Effects from Relics and Malfunctions list go here 
    }
    public void CreateHeadquarters(SetupGameManager _setup)
    {
        PlayerSpawn[] _spawns = FindObjectsOfType<PlayerSpawn>();

        for (int i = 0; i < _spawns.Length; i++)
        {
            GameObject _headquarters = Instantiate(HQPrefabs[i]);
            _headquarters.transform.position = _spawns[i].transform.position;

            HeadQuarters _hq = _headquarters.GetComponent<HeadQuarters>();

            _hq.owner = players[i];
            _hq.UpdatePlayerDetails();
            _hq.UpdatePlayerLocation();
            _hq.UpdateMaterials(); 
        }
        _setup.HeadquartersCompleted(); 

    }
}

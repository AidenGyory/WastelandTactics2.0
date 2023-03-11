using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupGameManager : MonoBehaviour
{
    public MapProfileTemplateSO mapProfile; 

    [SerializeField] MapShaper _mapShaper;
    [SerializeField] HexGridLayout _hexGridLayout;

    PlayerSpawn[] _spawns;

    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image loadingBar;
    [SerializeField] GameObject loadingCircle; 

    void Start()
    {
        Debug.Log("Setting up game..."); 
        SetupGame(); 
    }

    //Step 1
    void SetupGame()
    {
        loadingBar.fillAmount = 0.2f; 
        Debug.Log("Set up MapShaper");
        _mapShaper.SetMapShape(this, mapProfile);
        
    }

    //Step 2
    public void MapShaperCompleted()
    {
        loadingBar.fillAmount = 0.4f;
        Debug.Log("MapShape Complete!"); 
        Debug.Log("Set up Tilemap");
        _hexGridLayout.CreateHexTileMap(this, mapProfile);
         
    }

    //Step 3
    public void TileMapCompleted()
    {
        loadingBar.fillAmount = 0.6f;
        Debug.Log("Tile Map Complete!");
        GameManager.Instance.CreateHeadquarters(this); 
    }
    public void HeadquartersCompleted()
    {
        loadingBar.fillAmount = 0.8f;
        Debug.Log("Headquarters Complete!");
        Invoke("TurnOffShaper", 0.5f); 
    }

    public void TurnOffShaper()
    {
        SpawnRandomTile _tile = FindObjectOfType<SpawnRandomTile>(); 

        if(_tile != null)
        {
            Invoke("TurnOffShaper", 0.5f);
        }
        else
        {
            _mapShaper.TurnOff();
            Debug.Log("Map Setup Completed!!");
            HeadQuarters[] _hqs = FindObjectsOfType<HeadQuarters>();

            foreach (HeadQuarters _hq in _hqs)
            {
                _hq.SetOccupiedTile(); 
            }


            TileManager.instance.InitialiseAllTiles(); 
            GameManager.Instance.StartGame(); 
            
        }
        loadingBar.fillAmount = 1f;
        loadingScreen.GetComponent<Animator>().SetBool("Fade", true);
        loadingCircle.SetActive(false); 
    }
}

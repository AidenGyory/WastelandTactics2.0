using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public TileInfo[] tiles;

     
    private void Start()
    {
        Invoke("ReferenceAllTiles", 1f); 
    }
    public void ReferenceAllTiles()
    {
        TileInfo[] obj = FindObjectsOfType<TileInfo>();
        tiles = new TileInfo[obj.Length];
        tiles = obj; 
    }
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structures : MonoBehaviour
{
    public static Structures Instance;

    [Header("ClearTile")]
    public GameObject[] clearTile; 

    [Header("Player Headquarters")]
    public GameObject HQ;
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

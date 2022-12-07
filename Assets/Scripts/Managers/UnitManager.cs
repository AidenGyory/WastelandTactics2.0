using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    public enum unitTypes
    {
        Scout = 0
    }


    [Header("Unit Types")]
    public GameObject[] units;

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

    public void CreateUnit(unitTypes local_unitType, Transform local_position)
    {
        Debug.Log("Create Unit"); 
        GameObject _unit = Instantiate(units[(int)local_unitType]);
        _unit.transform.position = local_position.position; 
    }
}

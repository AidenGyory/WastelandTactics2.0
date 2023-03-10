using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapShaper : MonoBehaviour
{
    [SerializeField] GameObject[] shapes;
    [SerializeField] int _mapTypeAsIndex;
    [SerializeField] int _amountOfPlayers; 


    public void SetMapShape(SetupGameManager _setup, MapProfileTemplateSO _mapProfile)
    {

        if (_mapProfile != null)
        {
            _mapTypeAsIndex = (int)_mapProfile.mapType;
            _amountOfPlayers = _mapProfile.amountOfPlayers;
        }

        foreach (GameObject shape in shapes)
        {
            if (shape.activeInHierarchy)
            {
                shape.SetActive(false);
            }
        }

        shapes[_mapTypeAsIndex].SetActive(true);
        shapes[_mapTypeAsIndex].GetComponent<PlayerSpawnLocations>().TurnOnSpawnLocations(_amountOfPlayers - 2);

        _setup.MapShaperCompleted(); 
        //TurnOff(); 
    }

    public void TurnOff()
    {
        gameObject.SetActive(false); 
    }
}

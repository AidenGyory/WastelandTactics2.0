using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RelicAbilityScript : MonoBehaviour
{
    [SerializeField] GameObject RelicUI;

    private ResourceCacheTileScript tile; 

    public void TurnOnRelicUI(ResourceCacheTileScript _tile)
    {
        tile = _tile; 
        Time.timeScale = 0; 
        RelicUI.SetActive(true); 
    }

    public void ActivateRelic()
    {

        Time.timeScale = 1; 
        tile.GiveRelic(); 

    }

    public void ScrapRelic()
    {
        Time.timeScale = 1;
        tile.GiveMetal(100);
    }
}

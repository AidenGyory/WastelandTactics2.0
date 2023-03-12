using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RelicAbilityScript : MonoBehaviour
{
    [SerializeField] GameObject RelicUI;

    public void TurnOnRelicUI()
    {
        Time.timeScale = 0; 
        RelicUI.SetActive(true); 
    }
    public void ActivateRelic()
    {
        Time.timeScale = 1; 
        Debug.Log("Gain 1 Relic Abilty!");
        GameManager.Instance.currentPlayerTurn.AddRelic(); 

    }

    public void ScrapRelic()
    {
        Time.timeScale = 1;
        Debug.Log("Gain +100 Scrap Metal");
        GameManager.Instance.currentPlayerTurn.AddMetalScrap(100); 
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayHUDInfoScript : MonoBehaviour
{
    [SerializeField] TMP_Text EPLeft; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EPLeft.text = "" + GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn].ExplorationPointsLeft; 
    }


}

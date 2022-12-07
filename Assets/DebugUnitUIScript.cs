using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events; 

public class DebugUnitUIScript : MonoBehaviour
{
    [SerializeField] UnitInfo target;
    [Space]
    [SerializeField] TMP_Text unitName; 
    [SerializeField] TMP_Text movesLeft;
    [SerializeField] UnityEvent btn1;  
    [SerializeField] UnityEvent btn2;  
    [SerializeField] UnityEvent btn3;

    private void Start()
    {
        if(target != null)
        {
            unitName.text = "" + target.unitName;
        }
    }
    private void Update()
    {
        if (target != null)
        {
            movesLeft.text = "Moves Left: " + target.currentMovement + "/" + target.maxMovement;
        }
    }
    public void ButtonOnePressed()
    {
        btn1.Invoke();
    }

    public void ButtonTwoPressed()
    {
        btn2.Invoke(); 
    }

    public void ButtonThreePressed()
    {
        btn3.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DebugStructureUIScript : MonoBehaviour
{
    [SerializeField] StructureInfo target;
    [Space]
    [SerializeField] TMP_Text StructureName;
    [SerializeField] TMP_Text healthText; 
    [SerializeField] UnityEvent btn1;
    [SerializeField] UnityEvent btn2;
    [SerializeField] UnityEvent btn3;

    private void Start()
    {
        if(target != null)
        {
            StructureName.text = "" + target.structureName; 
        }
    }
    private void Update()
    {
        if (target != null)
        {
            healthText.text = "Health: " + target.currentHealth + "/" + target.maxHealth;
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
